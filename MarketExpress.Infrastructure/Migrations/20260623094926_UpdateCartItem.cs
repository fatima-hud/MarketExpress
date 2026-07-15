
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Color",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ColorHexCode",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Size",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "dbo",
                table: "CartItems",
                newName: "ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductId",
                schema: "dbo",
                table: "CartItems",
                newName: "IX_CartItems_ProductSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                schema: "dbo",
                table: "CartItems",
                column: "ProductSizeId",
                principalSchema: "dbo",
                principalTable: "ProductSizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "ProductSizeId",
                schema: "dbo",
                table: "CartItems",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductSizeId",
                schema: "dbo",
                table: "CartItems",
                newName: "IX_CartItems_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "dbo",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorHexCode",
                schema: "dbo",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                schema: "dbo",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                schema: "dbo",
                table: "CartItems",
                column: "ProductId",
                principalSchema: "dbo",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
