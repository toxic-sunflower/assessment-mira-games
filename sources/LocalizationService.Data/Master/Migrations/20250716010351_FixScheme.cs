using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalizationService.Data.Master.Migrations
{
    /// <inheritdoc />
    public partial class FixScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationKeyTranslation_Languages_LanguageId",
                table: "LocalizationKeyTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationKeyTranslation_LocalizationKey_LocalizationKeyId",
                table: "LocalizationKeyTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalizationKeyTranslation",
                table: "LocalizationKeyTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalizationKey",
                table: "LocalizationKey");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LocalizationKey");

            migrationBuilder.RenameTable(
                name: "LocalizationKeyTranslation",
                newName: "Translations");

            migrationBuilder.RenameTable(
                name: "LocalizationKey",
                newName: "LocalizationKeys");

            migrationBuilder.RenameIndex(
                name: "IX_LocalizationKeyTranslation_LocalizationKeyId",
                table: "Translations",
                newName: "IX_Translations_LocalizationKeyId");

            migrationBuilder.RenameIndex(
                name: "IX_LocalizationKeyTranslation_LanguageId",
                table: "Translations",
                newName: "IX_Translations_LanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Translations",
                table: "Translations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalizationKeys",
                table: "LocalizationKeys",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationKeys_Key",
                table: "LocalizationKeys",
                column: "Key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Languages_LanguageId",
                table: "Translations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_LocalizationKeys_LocalizationKeyId",
                table: "Translations",
                column: "LocalizationKeyId",
                principalTable: "LocalizationKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Languages_LanguageId",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_LocalizationKeys_LocalizationKeyId",
                table: "Translations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Translations",
                table: "Translations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalizationKeys",
                table: "LocalizationKeys");

            migrationBuilder.DropIndex(
                name: "IX_LocalizationKeys_Key",
                table: "LocalizationKeys");

            migrationBuilder.RenameTable(
                name: "Translations",
                newName: "LocalizationKeyTranslation");

            migrationBuilder.RenameTable(
                name: "LocalizationKeys",
                newName: "LocalizationKey");

            migrationBuilder.RenameIndex(
                name: "IX_Translations_LocalizationKeyId",
                table: "LocalizationKeyTranslation",
                newName: "IX_LocalizationKeyTranslation_LocalizationKeyId");

            migrationBuilder.RenameIndex(
                name: "IX_Translations_LanguageId",
                table: "LocalizationKeyTranslation",
                newName: "IX_LocalizationKeyTranslation_LanguageId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LocalizationKey",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalizationKeyTranslation",
                table: "LocalizationKeyTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalizationKey",
                table: "LocalizationKey",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationKeyTranslation_Languages_LanguageId",
                table: "LocalizationKeyTranslation",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationKeyTranslation_LocalizationKey_LocalizationKeyId",
                table: "LocalizationKeyTranslation",
                column: "LocalizationKeyId",
                principalTable: "LocalizationKey",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
