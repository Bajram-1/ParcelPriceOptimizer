using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPriceOptimizer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedDepthPropertyinCustomerInput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Depth",
                table: "CustomerInputs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "CustomerInputs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CustomerInputs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "CustomerInputs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "CustomerInputs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Depth",
                table: "CustomerInputs");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "CustomerInputs");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CustomerInputs");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "CustomerInputs");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "CustomerInputs");
        }
    }
}
