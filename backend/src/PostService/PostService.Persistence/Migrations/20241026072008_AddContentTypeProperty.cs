using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddContentTypeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Posts",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "ContentUrl");

            migrationBuilder.AddColumn<int>(
                name: "ContentType",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Posts",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ContentUrl",
                table: "Posts",
                newName: "Content");
        }
    }
}
