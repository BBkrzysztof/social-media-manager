using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaManager.Migrations
{
    /// <inheritdoc />
    public partial class SocialPlatformPlatformEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialAccounts_SocialPlatforms_SocialPlatformId",
                table: "SocialAccounts");

            migrationBuilder.RenameColumn(
                name: "PlatformId",
                table: "SocialAccounts",
                newName: "Platforms");

            migrationBuilder.AlterColumn<Guid>(
                name: "SocialPlatformId",
                table: "SocialAccounts",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAccounts_SocialPlatforms_SocialPlatformId",
                table: "SocialAccounts",
                column: "SocialPlatformId",
                principalTable: "SocialPlatforms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialAccounts_SocialPlatforms_SocialPlatformId",
                table: "SocialAccounts");

            migrationBuilder.RenameColumn(
                name: "Platforms",
                table: "SocialAccounts",
                newName: "PlatformId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SocialPlatformId",
                table: "SocialAccounts",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

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
