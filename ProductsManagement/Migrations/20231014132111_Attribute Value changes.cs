using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsManagement.Migrations
{
    /// <inheritdoc />
    public partial class AttributeValuechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_AttributeValues_ProductId",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "AttributesId",
                table: "AttributeValues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues",
                columns: new[] { "ProductId", "AttributeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "AttributeValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AttributesId",
                table: "AttributeValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues",
                columns: new[] { "ProductsId", "AttributesId" });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValues_ProductId",
                table: "AttributeValues",
                column: "ProductId");
        }
    }
}
