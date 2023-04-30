using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musference.Migrations
{
    public partial class Cloudinary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFile",
                table: "UsersDbSet",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "AudioFile",
                table: "TracksDbSet",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoFile",
                table: "TracksDbSet",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "AudioFile",
                table: "QuestionsDbSet",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageFile",
                table: "QuestionsDbSet",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "AudioFile",
                table: "AnswersDbSet",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageFile",
                table: "AnswersDbSet",
                type: "longtext",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFile",
                table: "UsersDbSet");

            migrationBuilder.DropColumn(
                name: "AudioFile",
                table: "TracksDbSet");

            migrationBuilder.DropColumn(
                name: "LogoFile",
                table: "TracksDbSet");

            migrationBuilder.DropColumn(
                name: "AudioFile",
                table: "QuestionsDbSet");

            migrationBuilder.DropColumn(
                name: "ImageFile",
                table: "QuestionsDbSet");

            migrationBuilder.DropColumn(
                name: "AudioFile",
                table: "AnswersDbSet");

            migrationBuilder.DropColumn(
                name: "ImageFile",
                table: "AnswersDbSet");
        }
    }
}
