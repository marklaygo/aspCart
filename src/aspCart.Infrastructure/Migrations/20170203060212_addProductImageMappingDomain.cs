using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aspCart.Infrastructure.Migrations
{
    public partial class addProductImageMappingDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductImageMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ImageId = table.Column<Guid>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImageMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImageMapping_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductImageMapping_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageMapping_ImageId",
                table: "ProductImageMapping",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageMapping_ProductId",
                table: "ProductImageMapping",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImageMapping");
        }
    }
}
