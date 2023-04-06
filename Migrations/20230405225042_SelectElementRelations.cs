using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sorting_App.Migrations
{
    /// <inheritdoc />
    public partial class SelectElementRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectElements_Sorts_SortID",
                table: "SelectElements");

            migrationBuilder.AlterColumn<int>(
                name: "SortID",
                table: "SelectElements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SelectElements_Sorts_SortID",
                table: "SelectElements",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectElements_Sorts_SortID",
                table: "SelectElements");

            migrationBuilder.AlterColumn<int>(
                name: "SortID",
                table: "SelectElements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectElements_Sorts_SortID",
                table: "SelectElements",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID");
        }
    }
}
