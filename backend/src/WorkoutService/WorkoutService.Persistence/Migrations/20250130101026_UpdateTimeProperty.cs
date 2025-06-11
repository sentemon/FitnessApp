using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Workouts");

            migrationBuilder.AddColumn<long>(
                name: "DurationInMinutes",
                table: "Workouts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInMinutes",
                table: "Workouts");

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "Workouts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
