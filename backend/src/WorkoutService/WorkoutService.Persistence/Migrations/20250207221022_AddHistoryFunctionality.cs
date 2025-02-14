using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Sets");

            migrationBuilder.CreateTable(
                name: "WorkoutHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DurationInMinutes = table.Column<long>(type: "bigint", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutHistories_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseHistories_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseHistories_WorkoutHistories_WorkoutHistoryId",
                        column: x => x.WorkoutHistoryId,
                        principalTable: "WorkoutHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reps = table.Column<long>(type: "bigint", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetHistories_ExerciseHistories_ExerciseHistoryId",
                        column: x => x.ExerciseHistoryId,
                        principalTable: "ExerciseHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistories_ExerciseId",
                table: "ExerciseHistories",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistories_WorkoutHistoryId",
                table: "ExerciseHistories",
                column: "WorkoutHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SetHistories_ExerciseHistoryId",
                table: "SetHistories",
                column: "ExerciseHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutHistories_UserId",
                table: "WorkoutHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutHistories_WorkoutId",
                table: "WorkoutHistories",
                column: "WorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetHistories");

            migrationBuilder.DropTable(
                name: "ExerciseHistories");

            migrationBuilder.DropTable(
                name: "WorkoutHistories");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Sets",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
