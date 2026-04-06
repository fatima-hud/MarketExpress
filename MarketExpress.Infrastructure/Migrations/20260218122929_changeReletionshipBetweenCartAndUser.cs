using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeReletionshipBetweenCartAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Stores_StoreId",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Stores_StoreId1",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId1",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_StoreId",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_StoreId1",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId1",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "StoreId",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "StoreId1",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "dto",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "dto",
                table: "Stores",
                newName: "StoreName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                schema: "dto",
                table: "Stores",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                schema: "dto",
                table: "Stores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeaturedImage",
                schema: "dto",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                schema: "dto",
                table: "Stores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                schema: "dto",
                table: "Carts",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                schema: "dto",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                schema: "dto",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "FeaturedImage",
                schema: "dto",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                schema: "dto",
                table: "Stores");

            migrationBuilder.RenameColumn(
                name: "StoreName",
                schema: "dto",
                table: "Stores",
                newName: "Name");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                schema: "dto",
                table: "Stores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                schema: "dto",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId1",
                schema: "dto",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                schema: "dto",
                table: "Carts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_StoreId",
                schema: "dto",
                table: "Carts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_StoreId1",
                schema: "dto",
                table: "Carts",
                column: "StoreId1");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                schema: "dto",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId1",
                schema: "dto",
                table: "Carts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Stores_StoreId",
                schema: "dto",
                table: "Carts",
                column: "StoreId",
                principalSchema: "dto",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Stores_StoreId1",
                schema: "dto",
                table: "Carts",
                column: "StoreId1",
                principalSchema: "dto",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId1",
                schema: "dto",
                table: "Carts",
                column: "UserId1",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
