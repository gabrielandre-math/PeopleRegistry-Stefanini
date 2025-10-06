using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleRegistry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonAddressColumns_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "People",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "People",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "People",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "People",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "People",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "People");

            migrationBuilder.DropColumn(
                name: "State",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "People");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "People");
        }
    }
}
