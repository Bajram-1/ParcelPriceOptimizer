using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPriceOptimizer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemovedShippingQuoteFromDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingQuotes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShippingQuotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    CalculatedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuotedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingQuotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingQuotes_Couriers_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Couriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingQuotes_CourierId",
                table: "ShippingQuotes",
                column: "CourierId");
        }
    }
}
