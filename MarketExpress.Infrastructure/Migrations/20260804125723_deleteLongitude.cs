using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteLongitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "dbo",
                table: "Stores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "dbo",
                table: "Stores",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "dbo",
                table: "Stores",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
