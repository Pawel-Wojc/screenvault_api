using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_screenvault.Migrations
{
    /// <inheritdoc />
    public partial class postcolumnamingchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Posts",
                newName: "Path");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Posts",
                newName: "Uri");
        }
    }
}
