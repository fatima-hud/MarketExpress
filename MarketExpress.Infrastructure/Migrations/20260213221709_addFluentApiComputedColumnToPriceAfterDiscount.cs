using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFluentApiComputedColumnToPriceAfterDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAfterDiscount",
                schema: "dto",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true,
                computedColumnSql: "CASE WHEN [DiscountPercentage] IS NOT NULL THEN [Price] - ([Price] * ([DiscountPercentage] / 100.0)) ELSE NULL END",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAfterDiscount",
                schema: "dto",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true,
                oldComputedColumnSql: "CASE WHEN [DiscountPercentage] IS NOT NULL THEN [Price] - ([Price] * ([DiscountPercentage] / 100.0)) ELSE NULL END");
        }
    }
}
