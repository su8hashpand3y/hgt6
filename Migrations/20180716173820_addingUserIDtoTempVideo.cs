using Microsoft.EntityFrameworkCore.Migrations;

namespace HGT6.Migrations
{
    public partial class addingUserIDtoTempVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "TempVideo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "TempVideo");
        }
    }
}
