using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTableStoreRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "dto",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                schema: "dto",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_StoreCategories_StoreCategoryID",
                schema: "dto",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreBusinessHours_Stores_StoreId",
                schema: "dto",
                table: "StoreBusinessHours");

            migrationBuilder.DropTable(
                name: "StoreCategories",
                schema: "dto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                schema: "dto",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedAT",
                schema: "dto",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                schema: "dto",
                table: "Stores");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "UserStoreRoles",
                schema: "dto",
                newName: "UserStoreRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserRefreshTokens",
                schema: "dto",
                newName: "UserRefreshTokens",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Stores",
                schema: "dto",
                newName: "Stores",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StoreBusinessHours",
                schema: "dto",
                newName: "StoreBusinessHours",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Ratings",
                schema: "dto",
                newName: "Ratings",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "dto",
                newName: "Products",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "OtpCodes",
                schema: "dto",
                newName: "OtpCodes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "dto",
                newName: "Orders",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                schema: "dto",
                newName: "Orderitems",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Clothingitems",
                schema: "dto",
                newName: "Clothingitems",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "dto",
                newName: "Categories",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Carts",
                schema: "dto",
                newName: "Carts",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "CartItems",
                schema: "dto",
                newName: "CartItems",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "StoreCategoryID",
                schema: "dbo",
                table: "Products",
                newName: "StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_StoreCategoryID",
                schema: "dbo",
                table: "Products",
                newName: "IX_Products_StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductId",
                schema: "dbo",
                table: "Orderitems",
                newName: "IX_Orderitems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderId",
                schema: "dbo",
                table: "Orderitems",
                newName: "IX_Orderitems_OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                schema: "dbo",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FeaturedImage",
                schema: "dbo",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                schema: "dbo",
                table: "Stores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreStatus",
                schema: "dbo",
                table: "Stores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkingHoursEnd",
                schema: "dbo",
                table: "Stores",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkingHoursStart",
                schema: "dbo",
                table: "Stores",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                schema: "dbo",
                table: "Products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2097152);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "dbo",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "dbo",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAfterDiscount",
                schema: "dbo",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true,
                computedColumnSql: "CASE WHEN [DiscountPercentage] IS NOT NULL THEN [Price] - ([Price] * ([DiscountPercentage] / 100.0)) ELSE [Price] END",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true,
                oldComputedColumnSql: "CASE WHEN [DiscountPercentage] IS NOT NULL THEN [Price] - ([Price] * ([DiscountPercentage] / 100.0)) ELSE NULL END",
                oldStored: null);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orderitems",
                schema: "dbo",
                table: "Orderitems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StoreRequests",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeaturedImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkingHoursStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkingHoursEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    NationalIdFrontImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalIdBackImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreRequests_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "dbo",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "dbo",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Discount",
                schema: "dbo",
                table: "Products",
                sql: "[DiscountPercentage] Is null or ([DiscountPercentage] >= 0 and [DiscountPercentage] <=100)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Price",
                schema: "dbo",
                table: "Products",
                sql: "[Price] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "dbo",
                table: "Products",
                sql: "[Quantity] >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_StoreRequests_Email",
                schema: "dbo",
                table: "StoreRequests",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreRequests_PhoneNumber",
                schema: "dbo",
                table: "StoreRequests",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreRequests_StoreId",
                schema: "dbo",
                table: "StoreRequests",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreRequests_StoreName",
                schema: "dbo",
                table: "StoreRequests",
                column: "StoreName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreRequests_UserName",
                schema: "dbo",
                table: "StoreRequests",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orderitems_Orders_OrderId",
                schema: "dbo",
                table: "Orderitems",
                column: "OrderId",
                principalSchema: "dbo",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orderitems_Products_ProductId",
                schema: "dbo",
                table: "Orderitems",
                column: "ProductId",
                principalSchema: "dbo",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "dbo",
                table: "Products",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stores_StoreId",
                schema: "dbo",
                table: "Products",
                column: "StoreId",
                principalSchema: "dbo",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreBusinessHours_Stores_StoreId",
                schema: "dbo",
                table: "StoreBusinessHours",
                column: "StoreId",
                principalSchema: "dbo",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orderitems_Orders_OrderId",
                schema: "dbo",
                table: "Orderitems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orderitems_Products_ProductId",
                schema: "dbo",
                table: "Orderitems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Stores_StoreId",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreBusinessHours_Stores_StoreId",
                schema: "dbo",
                table: "StoreBusinessHours");

            migrationBuilder.DropTable(
                name: "StoreRequests",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Discount",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Price",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Quantity",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orderitems",
                schema: "dbo",
                table: "Orderitems");

            migrationBuilder.DropColumn(
                name: "StoreStatus",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "WorkingHoursEnd",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "WorkingHoursStart",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "dbo",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "Categories");

            migrationBuilder.EnsureSchema(
                name: "dto");

            migrationBuilder.RenameTable(
                name: "UserStoreRoles",
                schema: "dbo",
                newName: "UserStoreRoles",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "UserRefreshTokens",
                schema: "dbo",
                newName: "UserRefreshTokens",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "Stores",
                schema: "dbo",
                newName: "Stores",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "StoreBusinessHours",
                schema: "dbo",
                newName: "StoreBusinessHours",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "Ratings",
                schema: "dbo",
                newName: "Ratings",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "dbo",
                newName: "Products",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "OtpCodes",
                schema: "dbo",
                newName: "OtpCodes",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "dbo",
                newName: "Orders",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "Orderitems",
                schema: "dbo",
                newName: "OrderItems",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "Clothingitems",
                schema: "dbo",
                newName: "Clothingitems",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "dbo",
                newName: "Categories",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "Carts",
                schema: "dbo",
                newName: "Carts",
                newSchema: "dto");

            migrationBuilder.RenameTable(
                name: "CartItems",
                schema: "dbo",
                newName: "CartItems",
                newSchema: "dto");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                schema: "dto",
                table: "Products",
                newName: "StoreCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_StoreId",
                schema: "dto",
                table: "Products",
                newName: "IX_Products_StoreCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Orderitems_ProductId",
                schema: "dto",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Orderitems_OrderId",
                schema: "dto",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                schema: "dto",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FeaturedImage",
                schema: "dto",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                schema: "dto",
                table: "Stores",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAT",
                schema: "dto",
                table: "Stores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                schema: "dto",
                table: "Stores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                schema: "dto",
                table: "Products",
                type: "nvarchar(max)",
                maxLength: 2097152,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAfterDiscount",
                schema: "dto",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true,
                computedColumnSql: "CASE WHEN [DiscountPercentage] IS NOT NULL THEN [Price] - ([Price] * ([DiscountPercentage] / 100.0)) ELSE NULL END",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true,
                oldComputedColumnSql: "CASE WHEN [DiscountPercentage] IS NOT NULL THEN [Price] - ([Price] * ([DiscountPercentage] / 100.0)) ELSE [Price] END");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                schema: "dto",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StoreCategories",
                schema: "dto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dto",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreCategories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "dto",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategories_CategoryId",
                schema: "dto",
                table: "StoreCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategories_StoreId",
                schema: "dto",
                table: "StoreCategories",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "dto",
                table: "OrderItems",
                column: "OrderId",
                principalSchema: "dto",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                schema: "dto",
                table: "OrderItems",
                column: "ProductId",
                principalSchema: "dto",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_StoreCategories_StoreCategoryID",
                schema: "dto",
                table: "Products",
                column: "StoreCategoryID",
                principalSchema: "dto",
                principalTable: "StoreCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreBusinessHours_Stores_StoreId",
                schema: "dto",
                table: "StoreBusinessHours",
                column: "StoreId",
                principalSchema: "dto",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
