using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Musference.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "TagsDbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagsDbSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersDbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    HashedPassword = table.Column<string>(type: "longtext", nullable: false),
                    Reputation = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Contact = table.Column<string>(type: "longtext", nullable: true),
                    City = table.Column<string>(type: "longtext", nullable: true),
                    Country = table.Column<string>(type: "longtext", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDbSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersDbSet_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsDbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Heading = table.Column<string>(type: "longtext", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pluses = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnswersAmount = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsDbSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsDbSet_UsersDbSet_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TracksDbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Artist = table.Column<string>(type: "longtext", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TimesListened = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TracksDbSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TracksDbSet_UsersDbSet_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswersDbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pluses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswersDbSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswersDbSet_QuestionsDbSet_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuestionsDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswersDbSet_UsersDbSet_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTag",
                columns: table => new
                {
                    QuestionsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTag", x => new { x.QuestionsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_QuestionTag_QuestionsDbSet_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "QuestionsDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTag_TagsDbSet_TagsId",
                        column: x => x.TagsId,
                        principalTable: "TagsDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionUser",
                columns: table => new
                {
                    QuestionsLikedId = table.Column<int>(type: "int", nullable: false),
                    UsersThatLikedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionUser", x => new { x.QuestionsLikedId, x.UsersThatLikedId });
                    table.ForeignKey(
                        name: "FK_QuestionUser_QuestionsDbSet_QuestionsLikedId",
                        column: x => x.QuestionsLikedId,
                        principalTable: "QuestionsDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionUser_UsersDbSet_UsersThatLikedId",
                        column: x => x.UsersThatLikedId,
                        principalTable: "UsersDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagTrack",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "int", nullable: false),
                    TracksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTrack", x => new { x.TagsId, x.TracksId });
                    table.ForeignKey(
                        name: "FK_TagTrack_TagsDbSet_TagsId",
                        column: x => x.TagsId,
                        principalTable: "TagsDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagTrack_TracksDbSet_TracksId",
                        column: x => x.TracksId,
                        principalTable: "TracksDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackUser",
                columns: table => new
                {
                    TracksLikedId = table.Column<int>(type: "int", nullable: false),
                    UsersThatLikedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackUser", x => new { x.TracksLikedId, x.UsersThatLikedId });
                    table.ForeignKey(
                        name: "FK_TrackUser_TracksDbSet_TracksLikedId",
                        column: x => x.TracksLikedId,
                        principalTable: "TracksDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackUser_UsersDbSet_UsersThatLikedId",
                        column: x => x.UsersThatLikedId,
                        principalTable: "UsersDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerUser",
                columns: table => new
                {
                    AnswersLikedId = table.Column<int>(type: "int", nullable: false),
                    UsersThatLikedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerUser", x => new { x.AnswersLikedId, x.UsersThatLikedId });
                    table.ForeignKey(
                        name: "FK_AnswerUser_AnswersDbSet_AnswersLikedId",
                        column: x => x.AnswersLikedId,
                        principalTable: "AnswersDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerUser_UsersDbSet_UsersThatLikedId",
                        column: x => x.UsersThatLikedId,
                        principalTable: "UsersDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswersDbSet_QuestionId",
                table: "AnswersDbSet",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswersDbSet_UserId",
                table: "AnswersDbSet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUser_UsersThatLikedId",
                table: "AnswerUser",
                column: "UsersThatLikedId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsDbSet_UserId",
                table: "QuestionsDbSet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTag_TagsId",
                table: "QuestionTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionUser_UsersThatLikedId",
                table: "QuestionUser",
                column: "UsersThatLikedId");

            migrationBuilder.CreateIndex(
                name: "IX_TagTrack_TracksId",
                table: "TagTrack",
                column: "TracksId");

            migrationBuilder.CreateIndex(
                name: "IX_TracksDbSet_UserId",
                table: "TracksDbSet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUser_UsersThatLikedId",
                table: "TrackUser",
                column: "UsersThatLikedId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDbSet_RoleId",
                table: "UsersDbSet",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerUser");

            migrationBuilder.DropTable(
                name: "QuestionTag");

            migrationBuilder.DropTable(
                name: "QuestionUser");

            migrationBuilder.DropTable(
                name: "TagTrack");

            migrationBuilder.DropTable(
                name: "TrackUser");

            migrationBuilder.DropTable(
                name: "AnswersDbSet");

            migrationBuilder.DropTable(
                name: "TagsDbSet");

            migrationBuilder.DropTable(
                name: "TracksDbSet");

            migrationBuilder.DropTable(
                name: "QuestionsDbSet");

            migrationBuilder.DropTable(
                name: "UsersDbSet");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
