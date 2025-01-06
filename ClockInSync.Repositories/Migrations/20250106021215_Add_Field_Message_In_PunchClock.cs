using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClockInSync.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Add_Field_Message_In_PunchClock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "PunchClocks",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "PunchClocks");
        }
    }
}
