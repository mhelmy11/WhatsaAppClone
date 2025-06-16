using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MessageType1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
               name: "MessageType",
               table: "Chats",
               type: "int",
               nullable: false,
               defaultValue: 0);

            migrationBuilder.AddColumn<int>(
              name: "MessageType",
              table: "Messages",
              type: "int",
              nullable: false,
              defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "Chats");

            migrationBuilder.DropColumn(
               name: "MessageType",
               table: "Messages");
        }
    }
}
