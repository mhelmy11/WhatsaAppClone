using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePrivacyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivacyException_UserPrivacySettings_OwnerUserId",
                table: "PrivacyException");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivacyException",
                table: "PrivacyException");

            migrationBuilder.RenameTable(
                name: "PrivacyException",
                newName: "PrivacyExceptions");

            migrationBuilder.RenameIndex(
                name: "IX_PrivacyException_ExcludedContactId",
                table: "PrivacyExceptions",
                newName: "IX_PrivacyExceptions_ExcludedContactId");

            migrationBuilder.AddColumn<string>(
                name: "AboutPrivacy",
                table: "UserPrivacySettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OnlinePrivacy",
                table: "UserPrivacySettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsExcludedFromAbout",
                table: "PrivacyExceptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivacyExceptions",
                table: "PrivacyExceptions",
                columns: new[] { "OwnerUserId", "ExcludedContactId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PrivacyExceptions_UserPrivacySettings_OwnerUserId",
                table: "PrivacyExceptions",
                column: "OwnerUserId",
                principalTable: "UserPrivacySettings",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivacyExceptions_UserPrivacySettings_OwnerUserId",
                table: "PrivacyExceptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivacyExceptions",
                table: "PrivacyExceptions");

            migrationBuilder.DropColumn(
                name: "AboutPrivacy",
                table: "UserPrivacySettings");

            migrationBuilder.DropColumn(
                name: "OnlinePrivacy",
                table: "UserPrivacySettings");

            migrationBuilder.DropColumn(
                name: "IsExcludedFromAbout",
                table: "PrivacyExceptions");

            migrationBuilder.RenameTable(
                name: "PrivacyExceptions",
                newName: "PrivacyException");

            migrationBuilder.RenameIndex(
                name: "IX_PrivacyExceptions_ExcludedContactId",
                table: "PrivacyException",
                newName: "IX_PrivacyException_ExcludedContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivacyException",
                table: "PrivacyException",
                columns: new[] { "OwnerUserId", "ExcludedContactId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PrivacyException_UserPrivacySettings_OwnerUserId",
                table: "PrivacyException",
                column: "OwnerUserId",
                principalTable: "UserPrivacySettings",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
