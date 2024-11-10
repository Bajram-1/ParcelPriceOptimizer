using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPriceOptimizer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenamedNameProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "FirstName");
        }
    }
}
