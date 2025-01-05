using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClockInSync.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Add_WeeklyJourney_In_Settings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeeklyJourney",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeeklyJourney",
                table: "Settings");
        }
    }
}
