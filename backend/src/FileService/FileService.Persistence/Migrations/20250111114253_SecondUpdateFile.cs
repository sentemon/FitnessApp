using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SecondUpdateFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlobName",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ForeignEntityId",
                table: "Files",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Files_BlobContainerName_BlobName",
                table: "Files",
                columns: new[] { "BlobContainerName", "BlobName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_BlobContainerName_BlobName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "BlobName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ForeignEntityId",
                table: "Files");
        }
    }
}
