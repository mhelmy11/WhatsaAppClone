using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPrivacyTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivacyException_UserPrivacySettings_OwnerUserId",
                table: "PrivacyException");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPrivacySettings",
                table: "UserPrivacySettings");

            migrationBuilder.DropIndex(
                name: "IX_UserPrivacySettings_UserId",
                table: "UserPrivacySettings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserPrivacySettings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPrivacySettings",
                table: "UserPrivacySettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivacyException_ExcludedContactId",
                table: "PrivacyException",
                column: "ExcludedContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivacyException_UserPrivacySettings_OwnerUserId",
                table: "PrivacyException",
                column: "OwnerUserId",
                principalTable: "UserPrivacySettings",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivacyException_UserPrivacySettings_OwnerUserId",
                table: "PrivacyException");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPrivacySettings",
                table: "UserPrivacySettings");

            migrationBuilder.DropIndex(
                name: "IX_PrivacyException_ExcludedContactId",
                table: "PrivacyException");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "UserPrivacySettings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPrivacySettings",
                table: "UserPrivacySettings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrivacySettings_UserId",
                table: "UserPrivacySettings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivacyException_UserPrivacySettings_OwnerUserId",
                table: "PrivacyException",
                column: "OwnerUserId",
                principalTable: "UserPrivacySettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
