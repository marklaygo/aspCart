using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aspCart.Infrastructure.Migrations
{
    public partial class addProductIdInOrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "OrderItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderItem");
        }
    }
}
