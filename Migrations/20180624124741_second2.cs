using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HGT6.Migrations
{
    public partial class second2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.AddColumn<long>(
                name: "Views",
                table: "Videos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "HGTUserID",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_HGTUserID",
                table: "Comments",
                column: "HGTUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_HGTUserID",
                table: "Comments",
                column: "HGTUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_HGTUserID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_HGTUserID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "HGTUserID",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Comments",
                nullable: true);
        }
    }
}
