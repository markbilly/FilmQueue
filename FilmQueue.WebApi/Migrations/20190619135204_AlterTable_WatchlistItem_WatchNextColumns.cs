using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmQueue.WebApi.Migrations
{
    public partial class AlterTable_WatchlistItem_WatchNextColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "WatchNextEnd",
                table: "WatchlistItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WatchNextStart",
                table: "WatchlistItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WatchedDateTime",
                table: "WatchlistItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WatchNextEnd",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "WatchNextStart",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "WatchedDateTime",
                table: "WatchlistItems");
        }
    }
}
