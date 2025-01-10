using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "BlobName",
                table: "Files",
                newName: "BlobContainerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlobContainerName",
                table: "Files",
                newName: "BlobName");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Files",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }
    }
}
