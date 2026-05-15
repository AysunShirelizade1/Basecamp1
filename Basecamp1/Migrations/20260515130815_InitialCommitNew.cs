using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basecamp1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommitNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Attachments",
                newName: "ContentType");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "Attachments",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "Attachments",
                newName: "FilePath");
        }
    }
}
