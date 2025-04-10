using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace H4SS.Migrations.TodoDb
{
    /// <inheritdoc />
    public partial class AddedUserIdFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Todolist_UserId",
                table: "Todolist",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todolist_Cpr_UserId",
                table: "Todolist",
                column: "UserId",
                principalTable: "Cpr",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todolist_Cpr_UserId",
                table: "Todolist");

            migrationBuilder.DropIndex(
                name: "IX_Todolist_UserId",
                table: "Todolist");
        }
    }
}
