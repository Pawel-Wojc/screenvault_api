using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_screenvault.Migrations
{
    /// <inheritdoc />
    public partial class postchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Posts",
                newName: "Uri");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LinkId",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LinkId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Posts",
                newName: "Link");
        }
    }
}
