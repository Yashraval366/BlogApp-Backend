using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.API.Migrations
{
    /// <inheritdoc />
    public partial class addedReactionTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_BlogsReactions_BlogId",
                table: "BlogsReactions");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "BlogsReactions");

            migrationBuilder.RenameTable(
                name: "BlogsReactions",
                newName: "BlogReaction");

            migrationBuilder.RenameIndex(
                name: "IX_BlogsReactions_UserId",
                table: "BlogReaction",
                newName: "IX_BlogReaction_UserId");

            migrationBuilder.AddColumn<int>(
                name: "ReactionType",
                table: "BlogReaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogReaction",
                table: "BlogReaction",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BlogReaction_BlogId_UserId",
                table: "BlogReaction",
                columns: new[] { "BlogId", "UserId" },
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_BlogReaction_BlogId_UserId",
                table: "BlogReaction");

            migrationBuilder.DropColumn(
                name: "ReactionType",
                table: "BlogReaction");

            migrationBuilder.RenameTable(
                name: "BlogReaction",
                newName: "BlogsReactions");

            migrationBuilder.RenameIndex(
                name: "IX_BlogReaction_UserId",
                table: "BlogsReactions",
                newName: "IX_BlogsReactions_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "BlogsReactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogsReactions",
                table: "BlogsReactions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BlogsReactions_BlogId",
                table: "BlogsReactions",
                column: "BlogId");

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
    }
}
