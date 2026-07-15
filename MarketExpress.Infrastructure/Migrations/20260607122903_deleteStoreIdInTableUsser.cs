using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteStoreIdInTableUsser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Stores_StoreId",
                schema: "identity",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Stores_StoreId",
                schema: "identity",
                table: "Users",
                column: "StoreId",
                principalSchema: "dbo",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Stores_StoreId",
                schema: "identity",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Stores_StoreId",
                schema: "identity",
                table: "Users",
                column: "StoreId",
                principalSchema: "dbo",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
