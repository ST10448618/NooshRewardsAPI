using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NooshRewardsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RewardRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    RequiredPunches = table.Column<int>(type: "INTEGER", nullable: false),
                    RewardDescription = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PunchCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    RewardRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentPunches = table.Column<int>(type: "INTEGER", nullable: false),
                    TimesRedeemed = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PunchCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PunchCards_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PunchCards_RewardRules_RewardRuleId",
                        column: x => x.RewardRuleId,
                        principalTable: "RewardRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    RewardRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiptReference = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PurchaseDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptSubmissions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptSubmissions_RewardRules_RewardRuleId",
                        column: x => x.RewardRuleId,
                        principalTable: "RewardRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScanTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Token = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    RewardRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScanTokens_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScanTokens_RewardRules_RewardRuleId",
                        column: x => x.RewardRuleId,
                        principalTable: "RewardRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PunchLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PunchCardId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScannedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Source = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PunchLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PunchLogs_PunchCards_PunchCardId",
                        column: x => x.PunchCardId,
                        principalTable: "PunchCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PunchCards_CustomerId_RewardRuleId",
                table: "PunchCards",
                columns: new[] { "CustomerId", "RewardRuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PunchCards_RewardRuleId",
                table: "PunchCards",
                column: "RewardRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PunchLogs_PunchCardId",
                table: "PunchLogs",
                column: "PunchCardId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptSubmissions_CustomerId",
                table: "ReceiptSubmissions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptSubmissions_ReceiptReference_AmountPaid_PurchaseDate",
                table: "ReceiptSubmissions",
                columns: new[] { "ReceiptReference", "AmountPaid", "PurchaseDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptSubmissions_RewardRuleId",
                table: "ReceiptSubmissions",
                column: "RewardRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanTokens_CustomerId",
                table: "ScanTokens",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanTokens_RewardRuleId",
                table: "ScanTokens",
                column: "RewardRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanTokens_Token",
                table: "ScanTokens",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PunchLogs");

            migrationBuilder.DropTable(
                name: "ReceiptSubmissions");

            migrationBuilder.DropTable(
                name: "ScanTokens");

            migrationBuilder.DropTable(
                name: "PunchCards");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "RewardRules");
        }
    }
}
