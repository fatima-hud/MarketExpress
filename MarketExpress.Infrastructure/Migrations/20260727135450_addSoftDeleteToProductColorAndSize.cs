using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSoftDeleteToProductColorAndSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "ProductSizes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "ProductColors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "ProductColors");
        }
    }
}
