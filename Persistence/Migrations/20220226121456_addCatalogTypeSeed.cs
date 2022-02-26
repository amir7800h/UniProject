using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class addCatalogTypeSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsRemoved",
                table: "CatalogType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRemoved",
                table: "CatalogBrand",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.InsertData(
                table: "CatalogBrand",
                columns: new[] { "Id", "Brand", "InsertTime", "RemoveTime", "UpdateTime" },
                values: new object[,]
                {
                    { 1, "سامسونگ", null, null, null },
                    { 2, "شیائومی ", null, null, null },
                    { 3, "اپل", null, null, null },
                    { 4, "هوآوی", null, null, null },
                    { 5, "نوکیا ", null, null, null },
                    { 6, "ال جی", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "CatalogType",
                columns: new[] { "Id", "InsertTime", "ParentCatalogTypeId", "RemoveTime", "Type", "UpdateTime" },
                values: new object[] { 1, null, null, null, "کالای دیجیتال", null });

            migrationBuilder.InsertData(
                table: "CatalogType",
                columns: new[] { "Id", "InsertTime", "ParentCatalogTypeId", "RemoveTime", "Type", "UpdateTime" },
                values: new object[] { 2, null, 1, null, "لوازم جانبی گوشی", null });

            migrationBuilder.InsertData(
                table: "CatalogType",
                columns: new[] { "Id", "InsertTime", "ParentCatalogTypeId", "RemoveTime", "Type", "UpdateTime" },
                values: new object[] { 3, null, 2, null, "پایه نگهدارنده گوشی", null });

            migrationBuilder.InsertData(
                table: "CatalogType",
                columns: new[] { "Id", "InsertTime", "ParentCatalogTypeId", "RemoveTime", "Type", "UpdateTime" },
                values: new object[] { 4, null, 2, null, "پاور بانک (شارژر همراه)", null });

            migrationBuilder.InsertData(
                table: "CatalogType",
                columns: new[] { "Id", "InsertTime", "ParentCatalogTypeId", "RemoveTime", "Type", "UpdateTime" },
                values: new object[] { 5, null, 2, null, "کیف و کاور گوشی", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CatalogType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CatalogType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CatalogType",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CatalogType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CatalogType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRemoved",
                table: "CatalogType",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRemoved",
                table: "CatalogBrand",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
