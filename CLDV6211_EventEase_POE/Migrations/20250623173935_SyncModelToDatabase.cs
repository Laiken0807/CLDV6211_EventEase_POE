using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CLDV6211_EventEase_POE.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_eventeases",
                table: "eventeases");

            migrationBuilder.RenameTable(
                name: "eventeases",
                newName: "Events");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "eventeases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_eventeases",
                table: "eventeases",
                column: "Id");
        }
    }
}
