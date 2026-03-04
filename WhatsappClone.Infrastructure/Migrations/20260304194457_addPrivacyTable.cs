using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPrivacyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPrivacySettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LastSeenPrivacy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicPrivacy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusPrivacy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrivacySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPrivacySettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivacyException",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerUserId = table.Column<long>(type: "bigint", nullable: false),
                    ExcludedContactId = table.Column<long>(type: "bigint", nullable: false),
                    IsExcludedFromProfilePic = table.Column<bool>(type: "bit", nullable: false),
                    IsExcludedFromLastSeen = table.Column<bool>(type: "bit", nullable: false),
                    IsExcludedFromStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacyException", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivacyException_UserPrivacySettings_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "UserPrivacySettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivacyException_OwnerUserId",
                table: "PrivacyException",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrivacySettings_UserId",
                table: "UserPrivacySettings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivacyException");

            migrationBuilder.DropTable(
                name: "UserPrivacySettings");
        }
    }
}
