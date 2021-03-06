﻿// <auto-generated />
using System;
using FilmQueue.WebApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FilmQueue.WebApi.Migrations
{
    [DbContext(typeof(FilmQueueDbContext))]
    [Migration("20190619135204_AlterTable_WatchlistItem_WatchNextColumns")]
    partial class AlterTable_WatchlistItem_WatchNextColumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FilmQueue.WebApi.DataAccess.Models.WatchlistItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedByUserId")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<int>("RuntimeInMinutes");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime?>("WatchNextEnd");

                    b.Property<DateTime?>("WatchNextStart");

                    b.Property<DateTime?>("WatchedDateTime");

                    b.HasKey("Id");

                    b.ToTable("WatchlistItems");
                });
#pragma warning restore 612, 618
        }
    }
}
