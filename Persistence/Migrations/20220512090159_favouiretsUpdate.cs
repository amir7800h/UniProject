using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class favouiretsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_CatalogItemFavourites_CatalogItemFavouriteId",
                table: "CatalogItems");

            migrationBuilder.DropIndex(
                name: "IX_CatalogItems_CatalogItemFavouriteId",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "CatalogItemFavouriteId",
                table: "CatalogItems");

            migrationBuilder.AddColumn<int>(
                name: "CatalogItemId",
                table: "CatalogItemFavourites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatalogItemId",
                table: "CatalogItemFavourites");

            migrationBuilder.AddColumn<int>(
                name: "CatalogItemFavouriteId",
                table: "CatalogItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CatalogItemFavouriteId",
                table: "CatalogItems",
                column: "CatalogItemFavouriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_CatalogItemFavourites_CatalogItemFavouriteId",
                table: "CatalogItems",
                column: "CatalogItemFavouriteId",
                principalTable: "CatalogItemFavourites",
                principalColumn: "Id");
        }
    }
}
