using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.API.Migrations
{
    /// <inheritdoc />
    public partial class addedReactionTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogReaction_AspNetUsers_UserId",
                table: "BlogReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogReaction_Blogs_BlogId",
                table: "BlogReaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogReaction",
                table: "BlogReaction");

            migrationBuilder.RenameTable(
                name: "BlogReaction",
                newName: "BlogsReactions");

            migrationBuilder.RenameIndex(
                name: "IX_BlogReaction_UserId",
                table: "BlogsReactions",
                newName: "IX_BlogsReactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogReaction_BlogId_UserId",
                table: "BlogsReactions",
                newName: "IX_BlogsReactions_BlogId_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogsReactions",
                table: "BlogsReactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogsReactions_AspNetUsers_UserId",
                table: "BlogsReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogsReactions_Blogs_BlogId",
                table: "BlogsReactions",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogsReactions_AspNetUsers_UserId",
                table: "BlogsReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogsReactions_Blogs_BlogId",
                table: "BlogsReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogsReactions",
                table: "BlogsReactions");

            migrationBuilder.RenameTable(
                name: "BlogsReactions",
                newName: "BlogReaction");

            migrationBuilder.RenameIndex(
                name: "IX_BlogsReactions_UserId",
                table: "BlogReaction",
                newName: "IX_BlogReaction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogsReactions_BlogId_UserId",
                table: "BlogReaction",
                newName: "IX_BlogReaction_BlogId_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogReaction",
                table: "BlogReaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogReaction_AspNetUsers_UserId",
                table: "BlogReaction",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogReaction_Blogs_BlogId",
                table: "BlogReaction",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
