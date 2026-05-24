using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationBetweenPaymentAndTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_PaymentId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentId",
                schema: "dbo",
                table: "Transactions",
                column: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_PaymentId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentId",
                schema: "dbo",
                table: "Transactions",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");
        }
    }
}
