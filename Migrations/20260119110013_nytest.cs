using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFGetStartedAgain.Migrations
{
    /// <inheritdoc />
    public partial class nytest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Tasks_TaskItemId",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "TaskItemId",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkerName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                });

            migrationBuilder.CreateTable(
                name: "TeamWorkers",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamWorkers", x => new { x.TeamId, x.WorkerId });
                    table.ForeignKey(
                        name: "FK_TeamWorkers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamWorkers_WorkerId",
                table: "TeamWorkers",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Tasks_TaskItemId",
                table: "Todos",
                column: "TaskItemId",
                principalTable: "Tasks",
                principalColumn: "TaskItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Tasks_TaskItemId",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "TeamWorkers");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.AlterColumn<int>(
                name: "TaskItemId",
                table: "Todos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Tasks_TaskItemId",
                table: "Todos",
                column: "TaskItemId",
                principalTable: "Tasks",
                principalColumn: "TaskItemId");
        }
    }
}
