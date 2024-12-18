﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPriceOptimizer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerInputForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerInputs_AspNetUsers_UserId",
                table: "CustomerInputs");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerInputs_AspNetUsers_UserId",
                table: "CustomerInputs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerInputs_AspNetUsers_UserId",
                table: "CustomerInputs");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerInputs_AspNetUsers_UserId",
                table: "CustomerInputs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
