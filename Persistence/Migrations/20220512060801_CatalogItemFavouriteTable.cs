using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class CatalogItemFavouriteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CatalogItemFavouriteId",
                table: "CatalogItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CatalogItemFavourites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemFavourites", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_CatalogItemFavourites_CatalogItemFavouriteId",
                table: "CatalogItems");

            migrationBuilder.DropTable(
                name: "CatalogItemFavourites");

            migrationBuilder.DropIndex(
                name: "IX_CatalogItems_CatalogItemFavouriteId",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "CatalogItemFavouriteId",
                table: "CatalogItems");
        }
    }
}
