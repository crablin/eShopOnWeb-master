using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.eShopWeb.Infrastructure.Data.Migrations
{
    public partial class AddedCategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("TRUNCATE TABLE [Catalog]", true);
            migrationBuilder.AddColumn<int>(
                name: "CatalogCategoryId",
                table: "Catalog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogCategoryId",
                table: "Catalog",
                column: "CatalogCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalog_CatalogCategory_CatalogCategoryId",
                table: "Catalog",
                column: "CatalogCategoryId",
                principalTable: "CatalogCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalog_CatalogCategory_CatalogCategoryId",
                table: "Catalog");

            migrationBuilder.DropIndex(
                name: "IX_Catalog_CatalogCategoryId",
                table: "Catalog");

            migrationBuilder.DropColumn(
                name: "CatalogCategoryId",
                table: "Catalog");
        }
    }
}
