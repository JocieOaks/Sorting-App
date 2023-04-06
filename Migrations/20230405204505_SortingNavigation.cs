using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sorting_App.Migrations
{
    /// <inheritdoc />
    public partial class SortingNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparison_Sorts_SortID",
                table: "ElementComparison");

            migrationBuilder.AlterColumn<int>(
                name: "SortID",
                table: "ElementComparison",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparison_Sorts_SortID",
                table: "ElementComparison",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparison_Sorts_SortID",
                table: "ElementComparison");

            migrationBuilder.AlterColumn<int>(
                name: "SortID",
                table: "ElementComparison",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparison_Sorts_SortID",
                table: "ElementComparison",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID");
        }
    }
}
