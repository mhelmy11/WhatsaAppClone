using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPrivacyTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivacyException",
                table: "PrivacyException");

            migrationBuilder.DropIndex(
                name: "IX_PrivacyException_OwnerUserId",
                table: "PrivacyException");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PrivacyException");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivacyException",
                table: "PrivacyException",
                columns: new[] { "OwnerUserId", "ExcludedContactId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivacyException",
                table: "PrivacyException");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "PrivacyException",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivacyException",
                table: "PrivacyException",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PrivacyException_OwnerUserId",
                table: "PrivacyException",
                column: "OwnerUserId");
        }
    }
}
