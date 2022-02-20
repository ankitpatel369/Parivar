using Microsoft.EntityFrameworkCore.Migrations;

namespace Parivar.Data.Migrations
{
    public partial class ChangeFamilyMemberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMemberDetails_RelationShipMasters_RelationShipId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "FamilyMemberDetails");

            migrationBuilder.AlterColumn<long>(
                name: "RelationShipId",
                table: "FamilyMemberDetails",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "BloodGroupId",
                table: "FamilyMemberDetails",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BussionessId",
                table: "FamilyMemberDetails",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EducationId",
                table: "FamilyMemberDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MosalVillage",
                table: "FamilyMemberDetails",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RelationShipMasterId",
                table: "FamilyMemberDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMemberDetails_BloodGroupId",
                table: "FamilyMemberDetails",
                column: "BloodGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMemberDetails_BussionessId",
                table: "FamilyMemberDetails",
                column: "BussionessId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMemberDetails_EducationId",
                table: "FamilyMemberDetails",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMemberDetails_RelationShipMasterId",
                table: "FamilyMemberDetails",
                column: "RelationShipMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMemberDetails_CategoriesMaster_BloodGroupId",
                table: "FamilyMemberDetails",
                column: "BloodGroupId",
                principalTable: "CategoriesMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMemberDetails_CategoriesMaster_BussionessId",
                table: "FamilyMemberDetails",
                column: "BussionessId",
                principalTable: "CategoriesMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMemberDetails_CategoriesMaster_EducationId",
                table: "FamilyMemberDetails",
                column: "EducationId",
                principalTable: "CategoriesMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMemberDetails_CategoriesMaster_RelationShipId",
                table: "FamilyMemberDetails",
                column: "RelationShipId",
                principalTable: "CategoriesMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMemberDetails_RelationShipMasters_RelationShipMasterId",
                table: "FamilyMemberDetails",
                column: "RelationShipMasterId",
                principalTable: "RelationShipMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMemberDetails_CategoriesMaster_BloodGroupId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMemberDetails_CategoriesMaster_BussionessId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMemberDetails_CategoriesMaster_EducationId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMemberDetails_CategoriesMaster_RelationShipId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMemberDetails_RelationShipMasters_RelationShipMasterId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropIndex(
                name: "IX_FamilyMemberDetails_BloodGroupId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropIndex(
                name: "IX_FamilyMemberDetails_BussionessId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropIndex(
                name: "IX_FamilyMemberDetails_EducationId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropIndex(
                name: "IX_FamilyMemberDetails_RelationShipMasterId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropColumn(
                name: "BloodGroupId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropColumn(
                name: "BussionessId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropColumn(
                name: "EducationId",
                table: "FamilyMemberDetails");

            migrationBuilder.DropColumn(
                name: "MosalVillage",
                table: "FamilyMemberDetails");

            migrationBuilder.DropColumn(
                name: "RelationShipMasterId",
                table: "FamilyMemberDetails");

            migrationBuilder.AlterColumn<long>(
                name: "RelationShipId",
                table: "FamilyMemberDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "FamilyMemberDetails",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMemberDetails_RelationShipMasters_RelationShipId",
                table: "FamilyMemberDetails",
                column: "RelationShipId",
                principalTable: "RelationShipMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
