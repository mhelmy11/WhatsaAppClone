using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addChatName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatName",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "ChatName",
                table: "Chats");


        }
    }
}
