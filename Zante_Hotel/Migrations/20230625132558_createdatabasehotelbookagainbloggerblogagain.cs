using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zante_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class createdatabasehotelbookagainbloggerblogagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_AithorId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AithorId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "AithorId",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AuthorId",
                table: "Blogs",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_AuthorId",
                table: "Blogs",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_AuthorId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AuthorId",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AithorId",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AithorId",
                table: "Blogs",
                column: "AithorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_AithorId",
                table: "Blogs",
                column: "AithorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
