using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CLDV6211_EventEase_POE.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTypeSeedAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventType_EventTypeId",
                table: "Event");

            migrationBuilder.InsertData(
                table: "EventType",
                columns: new[] { "EventTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Wedding" },
                    { 2, "Concert" },
                    { 3, "Birthday" },
                    { 4, "Corporate" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventType_EventTypeId",
                table: "Event",
                column: "EventTypeId",
                principalTable: "EventType",
                principalColumn: "EventTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventType_EventTypeId",
                table: "Event");

            migrationBuilder.DeleteData(
                table: "EventType",
                keyColumn: "EventTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EventType",
                keyColumn: "EventTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EventType",
                keyColumn: "EventTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EventType",
                keyColumn: "EventTypeId",
                keyValue: 4);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventType_EventTypeId",
                table: "Event",
                column: "EventTypeId",
                principalTable: "EventType",
                principalColumn: "EventTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
