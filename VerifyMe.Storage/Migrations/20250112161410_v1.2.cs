using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VerifyMe.Storage.Migrations
{
    /// <inheritdoc />
    public partial class v12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeAuths");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChallengeAuths",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ApplicationId = table.Column<long>(type: "INTEGER", nullable: true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeAuths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeAuths_Apps_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Apps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChallengeAuths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeAuths_ApplicationId",
                table: "ChallengeAuths",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeAuths_UserId",
                table: "ChallengeAuths",
                column: "UserId");
        }
    }
}
