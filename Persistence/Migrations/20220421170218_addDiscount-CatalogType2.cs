using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class addDiscountCatalogType2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItemDiscount_Discount_DiscountsId",
                table: "CatalogItemDiscount");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogTypeDiscount_Discount_DiscountsId",
                table: "CatalogTypeDiscount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount",
                table: "Discount");

            migrationBuilder.RenameTable(
                name: "Discount",
                newName: "Discounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItemDiscount_Discounts_DiscountsId",
                table: "CatalogItemDiscount",
                column: "DiscountsId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogTypeDiscount_Discounts_DiscountsId",
                table: "CatalogTypeDiscount",
                column: "DiscountsId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItemDiscount_Discounts_DiscountsId",
                table: "CatalogItemDiscount");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogTypeDiscount_Discounts_DiscountsId",
                table: "CatalogTypeDiscount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts");

            migrationBuilder.RenameTable(
                name: "Discounts",
                newName: "Discount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount",
                table: "Discount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItemDiscount_Discount_DiscountsId",
                table: "CatalogItemDiscount",
                column: "DiscountsId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogTypeDiscount_Discount_DiscountsId",
                table: "CatalogTypeDiscount",
                column: "DiscountsId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
