using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreBusinessHours",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StoreRequests",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserStoreRoles",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                schema: "identity",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StoreStatus",
                schema: "dbo",
                table: "Stores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                schema: "dbo",
                table: "Stores",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Stores",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                schema: "dbo",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                schema: "dbo",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                schema: "dbo",
                table: "Stores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_StoreId",
                schema: "identity",
                table: "Users",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_OwnerId",
                schema: "dbo",
                table: "Stores",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreName",
                schema: "dbo",
                table: "Stores",
                column: "StoreName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                schema: "dbo",
                table: "Documents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_OwnerId",
                schema: "dbo",
                table: "Stores",
                column: "OwnerId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Stores_StoreId",
                schema: "identity",
                table: "Users",
                column: "StoreId",
                principalSchema: "dbo",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_OwnerId",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Stores_StoreId",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Users_StoreId",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Stores_OwnerId",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_StoreName",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "StoreId",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Note",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                schema: "dbo",
                table: "Stores");

            migrationBuilder.AlterColumn<int>(
                name: "StoreStatus",
                schema: "dbo",
                table: "Stores",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

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

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "dbo",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StoreBusinessHours",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreBusinessHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreBusinessHours_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "dbo",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreRequests",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeaturedImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NationalIdBackImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalIdFrontImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    StoreName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkingHoursEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkingHoursStart = table.Column<TimeSpan>(type: "time", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "UserStoreRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStoreRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "dbo",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreBusinessHours_StoreId",
                schema: "dbo",
                table: "StoreBusinessHours",
                column: "StoreId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserStoreRoles_RoleId",
                schema: "dbo",
                table: "UserStoreRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStoreRoles_StoreId",
                schema: "dbo",
                table: "UserStoreRoles",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStoreRoles_UserId_RoleId_StoreId",
                schema: "dbo",
                table: "UserStoreRoles",
                columns: new[] { "UserId", "RoleId", "StoreId" },
                unique: true);
        }
    }
}
