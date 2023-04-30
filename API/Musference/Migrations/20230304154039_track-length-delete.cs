using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musference.Migrations
{
    public partial class tracklengthdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "TracksDbSet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "TracksDbSet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
