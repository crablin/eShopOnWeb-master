using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.eShopWeb.Infrastructure.Data.Migrations
{
    public partial class AddedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_category_hilo",
                incrementBy: 10);

            migrationBuilder.AddColumn<int>(
                name: "CatalogCategoryId",
                table: "CatalogType",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CatalogCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Category = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogType_CatalogCategoryId",
                table: "CatalogType",
                column: "CatalogCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogType_CatalogCategory_CatalogCategoryId",
                table: "CatalogType",
                column: "CatalogCategoryId",
                principalTable: "CatalogCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogType_CatalogCategory_CatalogCategoryId",
                table: "CatalogType");

            migrationBuilder.DropTable(
                name: "CatalogCategory");

            migrationBuilder.DropIndex(
                name: "IX_CatalogType_CatalogCategoryId",
                table: "CatalogType");

            migrationBuilder.DropSequence(
                name: "catalog_category_hilo");

            migrationBuilder.DropColumn(
                name: "CatalogCategoryId",
                table: "CatalogType");
        }
    }
}
