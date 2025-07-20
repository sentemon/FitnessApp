using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FollowCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FollowersCount",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FollowingCount",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowersCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FollowingCount",
                table: "Users");
        }
    }
}
