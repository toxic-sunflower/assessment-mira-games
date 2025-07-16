using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LocalizationService.Data.Read.Migrations
{
    /// <inheritdoc />
    public partial class CreateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Change",
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
                    table.PrimaryKey("PK_Change", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalizationKey",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
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

            migrationBuilder.CreateTable(
                name: "LocalizationKeyTranslation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<long>(type: "bigint", nullable: false),
                    LocalizationKeyId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationKeyTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalizationKeyTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocalizationKeyTranslation_LocalizationKey_LocalizationKeyId",
                        column: x => x.LocalizationKeyId,
                        principalTable: "LocalizationKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Change_Key",
                table: "Change",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Language_Code",
                table: "Language",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationKey_Key",
                table: "LocalizationKey",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationKeyTranslation_LanguageId",
                table: "LocalizationKeyTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationKeyTranslation_LocalizationKeyId",
                table: "LocalizationKeyTranslation",
                column: "LocalizationKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_Identifier",
                table: "Translations",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_Locale",
                table: "Translations",
                column: "Locale",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_LocalizationKey",
                table: "Translations",
                column: "LocalizationKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Change");

            migrationBuilder.DropTable(
                name: "LocalizationKeyTranslation");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "LocalizationKey");
        }
    }
}
