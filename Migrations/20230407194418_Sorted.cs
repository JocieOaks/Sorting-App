using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sorting_App.Migrations
{
    /// <inheritdoc />
    public partial class Sorted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSorted",
                table: "Sorts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSorted",
                table: "Sorts");
        }
    }
}
