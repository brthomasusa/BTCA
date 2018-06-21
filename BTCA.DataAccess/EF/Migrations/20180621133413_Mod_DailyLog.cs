using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BTCA.DataAccess.EF.Migrations
{
    public partial class Mod_DailyLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_AspNetUsers_Id",
                table: "Loads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loads",
                table: "Loads");

            migrationBuilder.RenameTable(
                name: "Loads",
                newName: "LoadAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_Loads_Id",
                table: "LoadAssignments",
                newName: "IX_LoadAssignments_Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DispatchDate",
                table: "LoadAssignments",
                nullable: false,
                defaultValue: new DateTime(2018, 6, 21, 8, 34, 12, 900, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 6, 18, 12, 11, 21, 903, DateTimeKind.Local));

            migrationBuilder.AddColumn<bool>(
                name: "IsSigned",
                table: "DailyLogs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoadAssignments",
                table: "LoadAssignments",
                column: "LoadID");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadAssignments_AspNetUsers_Id",
                table: "LoadAssignments",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadAssignments_AspNetUsers_Id",
                table: "LoadAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoadAssignments",
                table: "LoadAssignments");

            migrationBuilder.DropColumn(
                name: "IsSigned",
                table: "DailyLogs");

            migrationBuilder.RenameTable(
                name: "LoadAssignments",
                newName: "Loads");

            migrationBuilder.RenameIndex(
                name: "IX_LoadAssignments_Id",
                table: "Loads",
                newName: "IX_Loads_Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DispatchDate",
                table: "Loads",
                nullable: false,
                defaultValue: new DateTime(2018, 6, 18, 12, 11, 21, 903, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 6, 21, 8, 34, 12, 900, DateTimeKind.Local));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loads",
                table: "Loads",
                column: "LoadID");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_AspNetUsers_Id",
                table: "Loads",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
