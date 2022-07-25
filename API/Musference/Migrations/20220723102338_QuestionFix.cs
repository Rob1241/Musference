using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musference.Migrations
{
    public partial class QuestionFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "UsersDbSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "TagsDbSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Answers",
                table: "QuestionsDbSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "QuestionsDbSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "QuestionsDbSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AnswersDbSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TagsDbSet_QuestionId",
                table: "TagsDbSet",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagsDbSet_QuestionsDbSet_QuestionId",
                table: "TagsDbSet",
                column: "QuestionId",
                principalTable: "QuestionsDbSet",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagsDbSet_QuestionsDbSet_QuestionId",
                table: "TagsDbSet");

            migrationBuilder.DropIndex(
                name: "IX_TagsDbSet_QuestionId",
                table: "TagsDbSet");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "UsersDbSet");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "TagsDbSet");

            migrationBuilder.DropColumn(
                name: "Answers",
                table: "QuestionsDbSet");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "QuestionsDbSet");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "QuestionsDbSet");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AnswersDbSet");
        }
    }
}
