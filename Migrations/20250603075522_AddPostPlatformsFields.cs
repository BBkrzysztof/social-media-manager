using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaManager.Migrations
{
    /// <inheritdoc />
    public partial class AddPostPlatformsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Platforms",
                table: "Posts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Platforms",
                table: "Posts");
        }
    }
}
