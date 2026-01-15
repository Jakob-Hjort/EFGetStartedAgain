using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFGetStartedAgain.Migrations
{
    /// <inheritdoc />
    public partial class tasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Tasks_TaskId",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "Todos",
                newName: "TaskItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_TaskId",
                table: "Todos",
                newName: "IX_Todos_TaskItemId");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "Tasks",
                newName: "TaskItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Tasks_TaskItemId",
                table: "Todos",
                column: "TaskItemId",
                principalTable: "Tasks",
                principalColumn: "TaskItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Tasks_TaskItemId",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "TaskItemId",
                table: "Todos",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_TaskItemId",
                table: "Todos",
                newName: "IX_Todos_TaskId");

            migrationBuilder.RenameColumn(
                name: "TaskItemId",
                table: "Tasks",
                newName: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Tasks_TaskId",
                table: "Todos",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId");
        }
    }
}
