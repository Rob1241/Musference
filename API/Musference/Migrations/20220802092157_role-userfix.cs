using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musference.Migrations
{
    public partial class roleuserfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "UsersDbSet",
                newName: "HashedPassword");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UsersDbSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UsersDbSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UsersDbSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Reputation",
                table: "UsersDbSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserUser",
                columns: table => new
                {
                    FollowedUsersId = table.Column<int>(type: "int", nullable: false),
                    FollowingUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUser", x => new { x.FollowedUsersId, x.FollowingUsersId });
                    table.ForeignKey(
                        name: "FK_UserUser_UsersDbSet_FollowedUsersId",
                        column: x => x.FollowedUsersId,
                        principalTable: "UsersDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUser_UsersDbSet_FollowingUsersId",
                        column: x => x.FollowingUsersId,
                        principalTable: "UsersDbSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_FollowingUsersId",
                table: "UserUser",
                column: "FollowingUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserUser");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UsersDbSet");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UsersDbSet");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "UsersDbSet");

            migrationBuilder.DropColumn(
                name: "Reputation",
                table: "UsersDbSet");

            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "UsersDbSet",
                newName: "Password");
        }
    }
}
