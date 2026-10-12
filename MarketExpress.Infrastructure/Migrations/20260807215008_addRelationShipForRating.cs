using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRelationShipForRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_SocialMediaManagerUserId",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_SocialMediaManagerUserId",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "SocialMediaManagerUserId",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                schema: "dbo",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserId",
                schema: "dbo",
                table: "Ratings",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserId",
                schema: "dbo",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId",
                schema: "dbo",
                table: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaManagerUserId",
                schema: "dbo",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_SocialMediaManagerUserId",
                schema: "dbo",
                table: "Stores",
                column: "SocialMediaManagerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_SocialMediaManagerUserId",
                schema: "dbo",
                table: "Stores",
                column: "SocialMediaManagerUserId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
