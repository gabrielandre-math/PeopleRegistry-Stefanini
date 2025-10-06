using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PeopleRegistry.Application;
using PeopleRegistry.Domain.Entities;
using PeopleRegistry.Domain.Enums;
using PeopleRegistry.Domain.Security.Cryptography;
using PeopleRegistry.Infrastructure;
using PeopleRegistry.Infrastructure.DataAccess;
using System.Globalization;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers + filtro global de exceções
builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddEndpointsApiExplorer();

// Versionamento de API
builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0); // v1 é o padrão
});
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

// Swagger com autenticação JWT e múltiplas versões (v1 e v2)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PeopleRegistry API", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "PeopleRegistry API", Version = "v2" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT no header. Ex.: 'Bearer {token}'"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Localização
var supportedCultures = new List<CultureInfo> { new("en"), new("pt-BR"), new("es"), new("fr"), new("de") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Banco de dados (migrations estão no projeto Infrastructure)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PeopleRegistryDbContext>(options =>
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("PeopleRegistry.Infrastructure"))
);

// JWT – validações de config
var signingKey = builder.Configuration["Jwt:SigningKey"];
if (string.IsNullOrWhiteSpace(signingKey))
    throw new InvalidOperationException("JWT SigningKey not configured in appsettings.json");

var expStr = builder.Configuration["Jwt:ExpirationTimeMinutes"];
if (!uint.TryParse(expStr, out _))
    throw new InvalidOperationException("JWT ExpirationTimeMinutes not configured or invalid in appsettings.json");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

// Autenticação JWT
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});

// DI de infra e aplicação
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

// ---- SEED DE USUÁRIO ADMIN (migra e cria admin casoo necessário) ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PeopleRegistryDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();

    await db.Database.MigrateAsync();

    if (!await db.Users.AnyAsync())
    {
        var adminEmail = builder.Configuration["Seed:AdminEmail"] ?? "admin@company.com";
        var adminPass = builder.Configuration["Seed:AdminPassword"] ?? "Admin@123";

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Email = adminEmail.Trim().ToLowerInvariant(),
            PasswordHash = hasher.Encrypt(adminPass),
            Role = UserRole.Admin
        };

        await db.Users.AddAsync(admin);
        await db.SaveChangesAsync();
    }
}
// -----------------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PeopleRegistry API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "PeopleRegistry API v2");
    });
}

app.UseRequestLocalization();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
