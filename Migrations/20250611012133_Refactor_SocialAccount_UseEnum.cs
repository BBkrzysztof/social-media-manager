using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaManager.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_SocialAccount_UseEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialAccounts_SocialPlatforms_SocialPlatformId",
                table: "SocialAccounts");

            migrationBuilder.DropTable(
                name: "SocialPlatforms");

            migrationBuilder.DropIndex(
                name: "IX_SocialAccounts_SocialPlatformId",
                table: "SocialAccounts");

            migrationBuilder.DropColumn(
                name: "SocialPlatformId",
                table: "SocialAccounts");

            migrationBuilder.RenameColumn(
                name: "PlatformId",
                table: "SocialAccounts",
                newName: "Platform");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Platform",
                table: "SocialAccounts",
                newName: "PlatformId");

            migrationBuilder.AddColumn<Guid>(
                name: "SocialPlatformId",
                table: "SocialAccounts",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "SocialPlatforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Logo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialPlatforms", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SocialAccounts_SocialPlatformId",
                table: "SocialAccounts",
                column: "SocialPlatformId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAccounts_SocialPlatforms_SocialPlatformId",
                table: "SocialAccounts",
                column: "SocialPlatformId",
                principalTable: "SocialPlatforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
