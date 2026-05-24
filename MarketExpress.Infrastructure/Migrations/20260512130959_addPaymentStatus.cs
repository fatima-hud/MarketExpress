using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stores_StoreId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StoreId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StoreId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                schema: "dbo",
                table: "Orders",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stores_StoreId",
                schema: "dbo",
                table: "Orders",
                column: "StoreId",
                principalSchema: "dbo",
                principalTable: "Stores",
                principalColumn: "Id");
        }
    }
}
