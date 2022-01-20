using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class deleteMessageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageTypes_SendBy",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageTypes");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SendBy",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "TypeId",
                table: "Messages",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendBy",
                table: "Messages",
                column: "SendBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageTypes_SendBy",
                table: "Messages",
                column: "SendBy",
                principalTable: "MessageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
