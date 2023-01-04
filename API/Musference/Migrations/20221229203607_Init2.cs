using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musference.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "AnswersDbSet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AnswersDbSet",
                type: "longtext",
                nullable: false);
        }
    }
}
