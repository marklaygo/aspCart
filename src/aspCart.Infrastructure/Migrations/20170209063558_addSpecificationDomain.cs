using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aspCart.Infrastructure.Migrations
{
    public partial class addSpecificationDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Specification",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecificationMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    SpecificationId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecificationMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecificationMapping_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSpecificationMapping_Specification_SpecificationId",
                        column: x => x.SpecificationId,
                        principalTable: "Specification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationMapping_ProductId",
                table: "ProductSpecificationMapping",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationMapping_SpecificationId",
                table: "ProductSpecificationMapping",
                column: "SpecificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSpecificationMapping");

            migrationBuilder.DropTable(
                name: "Specification");
        }
    }
}
