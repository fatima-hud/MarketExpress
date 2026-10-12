using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSoftDeleteToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "identity",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "identity",
                table: "Users");
        }
    }
}
