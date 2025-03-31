using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeModel_Events_EventModelId",
                table: "EmployeeModel");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeModel_EventModelId",
                table: "EmployeeModel");

            migrationBuilder.DropColumn(
                name: "EventModelId",
                table: "EmployeeModel");

            migrationBuilder.CreateTable(
                name: "EventEmployee",
                columns: table => new
                {
                    EmployeesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEmployee", x => new { x.EmployeesId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_EventEmployee_EmployeeModel_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "EmployeeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEmployee_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventEmployee_EventsId",
                table: "EventEmployee",
                column: "EventsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventEmployee");

            migrationBuilder.AddColumn<Guid>(
                name: "EventModelId",
                table: "EmployeeModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeModel_EventModelId",
                table: "EmployeeModel",
                column: "EventModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeModel_Events_EventModelId",
                table: "EmployeeModel",
                column: "EventModelId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
