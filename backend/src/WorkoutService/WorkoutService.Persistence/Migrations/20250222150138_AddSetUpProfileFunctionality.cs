using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSetUpProfileFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ProfileSetUp",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileSetUp",
                table: "Users");
        }
    }
}
