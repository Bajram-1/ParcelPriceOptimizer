using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPriceOptimizer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MappedEntityRelationshipBetweenEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerInput_AspNetUsers_UserId1",
                table: "CustomerInput");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerInput_Parcel_ParcelId",
                table: "CustomerInput");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parcel",
                table: "Parcel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerInput",
                table: "CustomerInput");

            migrationBuilder.DropIndex(
                name: "IX_CustomerInput_ParcelId",
                table: "CustomerInput");

            migrationBuilder.DropIndex(
                name: "IX_CustomerInput_UserId1",
                table: "CustomerInput");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "CustomerInput");

            migrationBuilder.RenameTable(
                name: "Parcel",
                newName: "Parcels");

            migrationBuilder.RenameTable(
                name: "CustomerInput",
                newName: "CustomerInputs");

            migrationBuilder.AddColumn<int>(
                name: "CustomerInputId",
                table: "Parcels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CustomerInputs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parcels",
                table: "Parcels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerInputs",
                table: "CustomerInputs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourierPricingRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    MinVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DimensionPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeightPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierPricingRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourierPricingRules_Couriers_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Couriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingQuotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParcelId = table.Column<int>(type: "int", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_ShippingQuotes_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_CustomerInputId",
                table: "Parcels",
                column: "CustomerInputId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInputs_UserId",
                table: "CustomerInputs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierPricingRules_CourierId",
                table: "CourierPricingRules",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingQuotes_CourierId",
                table: "ShippingQuotes",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingQuotes_ParcelId",
                table: "ShippingQuotes",
                column: "ParcelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerInputs_AspNetUsers_UserId",
                table: "CustomerInputs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_CustomerInputs_CustomerInputId",
                table: "Parcels",
                column: "CustomerInputId",
                principalTable: "CustomerInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerInputs_AspNetUsers_UserId",
                table: "CustomerInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_CustomerInputs_CustomerInputId",
                table: "Parcels");

            migrationBuilder.DropTable(
                name: "CourierPricingRules");

            migrationBuilder.DropTable(
                name: "ShippingQuotes");

            migrationBuilder.DropTable(
                name: "Couriers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parcels",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_CustomerInputId",
                table: "Parcels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerInputs",
                table: "CustomerInputs");

            migrationBuilder.DropIndex(
                name: "IX_CustomerInputs_UserId",
                table: "CustomerInputs");

            migrationBuilder.DropColumn(
                name: "CustomerInputId",
                table: "Parcels");

            migrationBuilder.RenameTable(
                name: "Parcels",
                newName: "Parcel");

            migrationBuilder.RenameTable(
                name: "CustomerInputs",
                newName: "CustomerInput");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CustomerInput",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "CustomerInput",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parcel",
                table: "Parcel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerInput",
                table: "CustomerInput",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInput_ParcelId",
                table: "CustomerInput",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInput_UserId1",
                table: "CustomerInput",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerInput_AspNetUsers_UserId1",
                table: "CustomerInput",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerInput_Parcel_ParcelId",
                table: "CustomerInput",
                column: "ParcelId",
                principalTable: "Parcel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
