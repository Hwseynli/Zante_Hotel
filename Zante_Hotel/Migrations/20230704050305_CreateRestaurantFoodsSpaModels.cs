using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zante_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class CreateRestaurantFoodsSpaModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Restaurants_RestaurantId",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_RestaurantId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Foods");

            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "Foods",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Foods",
                newName: "About");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RestaurantFoods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantFoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantFoods_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantFoods_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantFoods_FoodId",
                table: "RestaurantFoods",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantFoods_RestaurantId",
                table: "RestaurantFoods",
                column: "RestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantFoods");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Foods",
                newName: "ImgUrl");

            migrationBuilder.RenameColumn(
                name: "About",
                table: "Foods",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "Foods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_RestaurantId",
                table: "Foods",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Restaurants_RestaurantId",
                table: "Foods",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
