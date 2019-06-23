using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmQueue.WebApi.Migrations
{
    public partial class AlterTable_WatchlistItems_Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("WatchlistItemRecords", newName: "FilmRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("FilmRecords", newName: "WatchlistItemRecords");
        }
    }
}
