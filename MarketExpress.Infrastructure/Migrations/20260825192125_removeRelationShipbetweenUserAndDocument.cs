using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeRelationShipbetweenUserAndDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_UserId",
                schema: "dbo",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_UserId",
                schema: "dbo",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "Documents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                schema: "dbo",
                table: "Documents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UserId",
                schema: "dbo",
                table: "Documents",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
