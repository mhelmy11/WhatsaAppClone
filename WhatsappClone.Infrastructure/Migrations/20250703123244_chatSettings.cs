using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chatSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChatSettings_AspNetUsers_ReceiverId",
                table: "UserChatSettings");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "UserChatSettings",
                newName: "ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatSettings_ReceiverId",
                table: "UserChatSettings",
                newName: "IX_UserChatSettings_ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatSettings_AspNetUsers_ContactId",
                table: "UserChatSettings",
                column: "ContactId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChatSettings_AspNetUsers_ContactId",
                table: "UserChatSettings");

            migrationBuilder.RenameColumn(
                name: "ContactId",
                table: "UserChatSettings",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatSettings_ContactId",
                table: "UserChatSettings",
                newName: "IX_UserChatSettings_ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatSettings_AspNetUsers_ReceiverId",
                table: "UserChatSettings",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
