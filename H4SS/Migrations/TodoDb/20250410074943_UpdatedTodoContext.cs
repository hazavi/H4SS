using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace H4SS.Migrations.TodoDb
{
    /// <inheritdoc />
    public partial class UpdatedTodoContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todolist_Cpr_UserId",
                table: "Todolist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todolist",
                table: "Todolist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cpr",
                table: "Cpr");

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "Todolist",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "Cpr",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CprNr",
                table: "Cpr",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Todolist__3214EC07191F8783",
                table: "Todolist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Cpr__3214EC07B603CEB1",
                table: "Cpr",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todolist_Cpr",
                table: "Todolist",
                column: "UserId",
                principalTable: "Cpr",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todolist_Cpr",
                table: "Todolist");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Todolist__3214EC07191F8783",
                table: "Todolist");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Cpr__3214EC07B603CEB1",
                table: "Cpr");

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "Todolist",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "Cpr",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CprNr",
                table: "Cpr",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todolist",
                table: "Todolist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cpr",
                table: "Cpr",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todolist_Cpr_UserId",
                table: "Todolist",
                column: "UserId",
                principalTable: "Cpr",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
