using Microsoft.EntityFrameworkCore.Migrations;

namespace Bloggster.Migrations
{
    public partial class ModelCleanup1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_users_UserID",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_UserID",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "posts");

            migrationBuilder.AddColumn<bool>(
                name: "isAdmin",
                table: "users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_posts_BloggerID",
                table: "posts",
                column: "BloggerID");

            migrationBuilder.AddForeignKey(
                name: "FK_posts_users_BloggerID",
                table: "posts",
                column: "BloggerID",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_users_BloggerID",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_BloggerID",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_posts_UserID",
                table: "posts",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_posts_users_UserID",
                table: "posts",
                column: "UserID",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
