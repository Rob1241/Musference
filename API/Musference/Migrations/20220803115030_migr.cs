using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musference.Migrations
{
    public partial class migr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Answers",
                table: "QuestionsDbSet",
                newName: "AnswersAmount");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "UsersDbSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "AnswersDbSet",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDbSet_RoleId",
                table: "UsersDbSet",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswersDbSet_QuestionId",
                table: "AnswersDbSet",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswersDbSet_UserId",
                table: "AnswersDbSet",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswersDbSet_QuestionsDbSet_QuestionId",
                table: "AnswersDbSet",
                column: "QuestionId",
                principalTable: "QuestionsDbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswersDbSet_UsersDbSet_UserId",
                table: "AnswersDbSet",
                column: "UserId",
                principalTable: "UsersDbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersDbSet_Roles_RoleId",
                table: "UsersDbSet",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswersDbSet_QuestionsDbSet_QuestionId",
                table: "AnswersDbSet");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswersDbSet_UsersDbSet_UserId",
                table: "AnswersDbSet");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersDbSet_Roles_RoleId",
                table: "UsersDbSet");

            migrationBuilder.DropIndex(
                name: "IX_UsersDbSet_RoleId",
                table: "UsersDbSet");

            migrationBuilder.DropIndex(
                name: "IX_AnswersDbSet_QuestionId",
                table: "AnswersDbSet");

            migrationBuilder.DropIndex(
                name: "IX_AnswersDbSet_UserId",
                table: "AnswersDbSet");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UsersDbSet");

            migrationBuilder.RenameColumn(
                name: "AnswersAmount",
                table: "QuestionsDbSet",
                newName: "Answers");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionId",
                table: "AnswersDbSet",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
