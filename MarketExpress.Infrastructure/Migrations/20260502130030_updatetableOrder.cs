using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetableOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stores_StoreId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Statue",
                schema: "dbo",
                table: "Orders",
                newName: "Status");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ColorHexCode",
                schema: "dbo",
                table: "Orderitems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorHexCode",
                schema: "dbo",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stores_StoreId",
                schema: "dbo",
                table: "Orders",
                column: "StoreId",
                principalSchema: "dbo",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stores_StoreId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ColorHexCode",
                schema: "dbo",
                table: "Orderitems");

            migrationBuilder.DropColumn(
                name: "ColorHexCode",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "Orders",
                newName: "Statue");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stores_StoreId",
                schema: "dbo",
                table: "Orders",
                column: "StoreId",
                principalSchema: "dbo",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
