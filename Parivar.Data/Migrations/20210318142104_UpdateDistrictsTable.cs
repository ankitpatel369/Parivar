using Microsoft.EntityFrameworkCore.Migrations;

namespace Parivar.Data.Migrations
{
    public partial class UpdateDistrictsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "Districts",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_StateId",
                table: "Districts",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_States_StateId",
                table: "Districts",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_States_StateId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_StateId",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Districts");
        }
    }
}
