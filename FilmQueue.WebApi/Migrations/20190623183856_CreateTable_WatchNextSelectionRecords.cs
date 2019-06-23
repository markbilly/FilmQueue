using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmQueue.WebApi.Migrations
{
    public partial class CreateTable_WatchNextSelectionRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatchNextSelectionRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    FilmId = table.Column<long>(nullable: false),
                    SelectedDateTime = table.Column<DateTime>(nullable: false),
                    ExpiredDateTime = table.Column<DateTime>(nullable: true),
                    Watched = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchNextSelectionRecords", x => x.Id);
                });

            migrationBuilder.Sql(@"
                INSERT INTO
	                WatchNextSelectionRecords (UserId, FilmId, SelectedDateTime, ExpiredDateTime, Watched)
                SELECT
	                 FR.CreatedByUserId AS UserId
                    ,FR.Id AS FilmId
                    ,FR.WatchNextStart AS SelectedDateTime
                    ,FR.WatchNextEnd AS ExpiredDateTime
                    ,CASE WHEN FR.WatchNextEnd IS NOT NULL THEN 1 ELSE 0 END AS Watched
                FROM
	                FilmRecords AS FR
                WHERE
	                FR.WatchNextStart IS NOT NULL");

            migrationBuilder.DropColumn(
                name: "WatchNextEnd",
                table: "FilmRecords");

            migrationBuilder.DropColumn(
                name: "WatchNextStart",
                table: "FilmRecords");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "FilmRecords",
                newName: "OwnedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchNextSelectionRecords");

            migrationBuilder.RenameColumn(
                name: "OwnedByUserId",
                table: "FilmRecords",
                newName: "CreatedByUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "WatchNextEnd",
                table: "FilmRecords",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WatchNextStart",
                table: "FilmRecords",
                nullable: true);
        }
    }
}
