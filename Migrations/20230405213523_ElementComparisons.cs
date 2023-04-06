using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sorting_App.Migrations
{
    /// <inheritdoc />
    public partial class ElementComparisons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparison_SelectElement_FirstElementID",
                table: "ElementComparison");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparison_SelectElement_SecondElementID",
                table: "ElementComparison");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparison_Sorts_SortID",
                table: "ElementComparison");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElementComparison",
                table: "ElementComparison");

            migrationBuilder.RenameTable(
                name: "ElementComparison",
                newName: "ElementComparisons");

            migrationBuilder.RenameIndex(
                name: "IX_ElementComparison_SortID",
                table: "ElementComparisons",
                newName: "IX_ElementComparisons_SortID");

            migrationBuilder.RenameIndex(
                name: "IX_ElementComparison_SecondElementID",
                table: "ElementComparisons",
                newName: "IX_ElementComparisons_SecondElementID");

            migrationBuilder.RenameIndex(
                name: "IX_ElementComparison_FirstElementID",
                table: "ElementComparisons",
                newName: "IX_ElementComparisons_FirstElementID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElementComparisons",
                table: "ElementComparisons",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_SelectElement_FirstElementID",
                table: "ElementComparisons",
                column: "FirstElementID",
                principalTable: "SelectElement",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_SelectElement_SecondElementID",
                table: "ElementComparisons",
                column: "SecondElementID",
                principalTable: "SelectElement",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_Sorts_SortID",
                table: "ElementComparisons",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SelectElement_FirstElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SelectElement_SecondElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_Sorts_SortID",
                table: "ElementComparisons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElementComparisons",
                table: "ElementComparisons");

            migrationBuilder.RenameTable(
                name: "ElementComparisons",
                newName: "ElementComparison");

            migrationBuilder.RenameIndex(
                name: "IX_ElementComparisons_SortID",
                table: "ElementComparison",
                newName: "IX_ElementComparison_SortID");

            migrationBuilder.RenameIndex(
                name: "IX_ElementComparisons_SecondElementID",
                table: "ElementComparison",
                newName: "IX_ElementComparison_SecondElementID");

            migrationBuilder.RenameIndex(
                name: "IX_ElementComparisons_FirstElementID",
                table: "ElementComparison",
                newName: "IX_ElementComparison_FirstElementID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElementComparison",
                table: "ElementComparison",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparison_SelectElement_FirstElementID",
                table: "ElementComparison",
                column: "FirstElementID",
                principalTable: "SelectElement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparison_SelectElement_SecondElementID",
                table: "ElementComparison",
                column: "SecondElementID",
                principalTable: "SelectElement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparison_Sorts_SortID",
                table: "ElementComparison",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
