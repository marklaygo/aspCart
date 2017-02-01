using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aspCart.Infrastructure.Migrations
{
    public partial class addProductDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    SpecialPrice = table.Column<decimal>(nullable: true),
                    SpecialPriceStartDate = table.Column<DateTime>(nullable: true),
                    SpecialPriceEndDate = table.Column<DateTime>(nullable: true),
                    StockQuantity = table.Column<int>(nullable: false),
                    MinimumStockQuantity = table.Column<int>(nullable: false),
                    NotifyForQuantityBelow = table.Column<int>(nullable: false),
                    DisplayAvailability = table.Column<bool>(nullable: false),
                    MinimumCartQuantity = table.Column<int>(nullable: false),
                    MaximumCartQuantity = table.Column<int>(nullable: false),
                    SeoUrl = table.Column<string>(nullable: true),
                    MetaTitle = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    Published = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryMapping_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategoryMapping_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryMapping_CategoryId",
                table: "ProductCategoryMapping",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryMapping_ProductId",
                table: "ProductCategoryMapping",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategoryMapping");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
