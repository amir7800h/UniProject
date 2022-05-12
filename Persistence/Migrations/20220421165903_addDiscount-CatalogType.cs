using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class addDiscountCatalogType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discountlimitationId",
                table: "Discount",
                newName: "DiscountLimitationId");

            migrationBuilder.CreateTable(
                name: "CatalogTypeDiscount",
                columns: table => new
                {
                    CatalogTypesId = table.Column<int>(type: "int", nullable: false),
                    DiscountsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTypeDiscount", x => new { x.CatalogTypesId, x.DiscountsId });
                    table.ForeignKey(
                        name: "FK_CatalogTypeDiscount_CatalogType_CatalogTypesId",
                        column: x => x.CatalogTypesId,
                        principalTable: "CatalogType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogTypeDiscount_Discount_DiscountsId",
                        column: x => x.DiscountsId,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTypeDiscount_DiscountsId",
                table: "CatalogTypeDiscount",
                column: "DiscountsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogTypeDiscount");

            migrationBuilder.RenameColumn(
                name: "DiscountLimitationId",
                table: "Discount",
                newName: "discountlimitationId");
        }
    }
}
