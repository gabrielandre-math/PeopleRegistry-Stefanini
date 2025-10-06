using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var desc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new OpenApiInfo
            {
                Title = "PeopleRegistry API",
                Version = desc.GroupName
            });
        }

        options.DocInclusionPredicate((docName, apiDesc) =>
        {
            var model = apiDesc.ActionDescriptor
                .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

            if (model is null)
                return true;

            bool Matches(ApiVersion v)
            {
                var vMajor = $"v{v.MajorVersion}";     // ex.: v1
                var vFull = $"v{v}";                  // ex.: v1.0
                return docName == vMajor || docName == vFull;
            }

            return model.DeclaredApiVersions.Any(Matches)
                || model.ImplementedApiVersions.Any(Matches);
        });

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
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id   = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}
