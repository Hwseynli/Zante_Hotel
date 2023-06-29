using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zante_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class createdatabasehotelbookHotelModelsCreateagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "Hotels",
                newName: "WebSite");

            migrationBuilder.AddColumn<string>(
                name: "MapLink",
                table: "Hotels",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapLink",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "WebSite",
                table: "Hotels",
                newName: "City");
        }
    }
}
