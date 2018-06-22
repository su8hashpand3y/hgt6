using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HGT6.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "FolderName",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Poster",
                table: "Videos");

            migrationBuilder.AddColumn<string>(
                name: "PosterUrl",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Videos");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FolderName",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Poster",
                table: "Videos",
                nullable: true);
        }
    }
}
