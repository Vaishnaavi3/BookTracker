using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookTracker.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    PublishedOn = table.Column<DateTime>(nullable: false),
                    Pages = table.Column<int>(nullable: false),
                    Genre = table.Column<int>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Rating = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    EnrollId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    EmailId = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.EnrollId);
                });

            migrationBuilder.CreateTable(
                name: "UserShelf",
                columns: table => new
                {
                    Serialno = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(nullable: true),
                    BookId = table.Column<int>(nullable: true),
                    UserDetailsEnrollId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShelf", x => x.Serialno);
                    table.ForeignKey(
                        name: "FK_UserShelf_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserShelf_UserDetails_UserDetailsEnrollId",
                        column: x => x.UserDetailsEnrollId,
                        principalTable: "UserDetails",
                        principalColumn: "EnrollId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserShelf_BookId",
                table: "UserShelf",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserShelf_UserDetailsEnrollId",
                table: "UserShelf",
                column: "UserDetailsEnrollId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserShelf");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "UserDetails");
        }
    }
}
