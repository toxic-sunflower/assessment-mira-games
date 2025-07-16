using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalizationService.Data.Master.Migrations
{
    /// <inheritdoc />
    public partial class AddChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Changes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    CurrentState = table.Column<string>(type: "jsonb", nullable: false),
                    NewState = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Changes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    LocalizationKey = table.Column<string>(type: "text", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_Changes_Key",
                table: "Changes",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translation_Identifier",
                table: "Translation",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translation_Locale",
                table: "Translation",
                column: "Locale",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translation_LocalizationKey",
                table: "Translation",
                column: "LocalizationKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Changes");

            migrationBuilder.DropTable(
                name: "Translation");
        }
    }
}
