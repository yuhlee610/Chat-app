using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class addSendByUserInMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "SendBy",
                table: "Messages",
                newName: "SendByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendByUserId",
                table: "Messages",
                column: "SendByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SendByUserId",
                table: "Messages",
                column: "SendByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SendByUserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SendByUserId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "SendByUserId",
                table: "Messages",
                newName: "SendBy");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Messages",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
