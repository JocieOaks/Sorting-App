using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sorting_App.Migrations
{
    /// <inheritdoc />
    public partial class SelectElement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SelectElement_FirstElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SelectElement_SecondElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectElement_Elements_ElementID",
                table: "SelectElement");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectElement_Sorts_SortID",
                table: "SelectElement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SelectElement",
                table: "SelectElement");

            migrationBuilder.RenameTable(
                name: "SelectElement",
                newName: "SelectElements");

            migrationBuilder.RenameIndex(
                name: "IX_SelectElement_SortID",
                table: "SelectElements",
                newName: "IX_SelectElements_SortID");

            migrationBuilder.RenameIndex(
                name: "IX_SelectElement_ElementID",
                table: "SelectElements",
                newName: "IX_SelectElements_ElementID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SelectElements",
                table: "SelectElements",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_SelectElements_FirstElementID",
                table: "ElementComparisons",
                column: "FirstElementID",
                principalTable: "SelectElements",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_SelectElements_SecondElementID",
                table: "ElementComparisons",
                column: "SecondElementID",
                principalTable: "SelectElements",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectElements_Elements_ElementID",
                table: "SelectElements",
                column: "ElementID",
                principalTable: "Elements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SelectElements_Sorts_SortID",
                table: "SelectElements",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SelectElements_FirstElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SelectElements_SecondElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectElements_Elements_ElementID",
                table: "SelectElements");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectElements_Sorts_SortID",
                table: "SelectElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SelectElements",
                table: "SelectElements");

            migrationBuilder.RenameTable(
                name: "SelectElements",
                newName: "SelectElement");

            migrationBuilder.RenameIndex(
                name: "IX_SelectElements_SortID",
                table: "SelectElement",
                newName: "IX_SelectElement_SortID");

            migrationBuilder.RenameIndex(
                name: "IX_SelectElements_ElementID",
                table: "SelectElement",
                newName: "IX_SelectElement_ElementID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SelectElement",
                table: "SelectElement",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_SelectElement_FirstElementID",
                table: "ElementComparisons",
                column: "FirstElementID",
                principalTable: "SelectElement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_SelectElement_SecondElementID",
                table: "ElementComparisons",
                column: "SecondElementID",
                principalTable: "SelectElement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SelectElement_Elements_ElementID",
                table: "SelectElement",
                column: "ElementID",
                principalTable: "Elements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SelectElement_Sorts_SortID",
                table: "SelectElement",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID");
        }
    }
}
