using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Statuses_StatusID",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_StatusID",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusID",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StatusID",
                table: "Messages",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Statuses_StatusID",
                table: "Messages",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID");
        }
    }
}
