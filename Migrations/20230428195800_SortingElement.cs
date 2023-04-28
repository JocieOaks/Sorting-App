using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sorting_App.Migrations
{
    /// <inheritdoc />
    public partial class SortingElement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SelectElements_FirstElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SelectElements_SecondElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_Sorts_SortID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_Elements_ElementLists_ListID",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Sorts_ElementLists_ElementListID",
                table: "Sorts");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ElementLists_ListID",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "SelectElements");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ListID",
                table: "Tags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ElementListID",
                table: "Sorts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Elements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ListID",
                table: "Elements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SortID",
                table: "ElementComparisons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "SortingElements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElementID = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    LastTimeAsOption = table.Column<int>(type: "int", nullable: false),
                    NumberOfTimesAsOption = table.Column<int>(type: "int", nullable: false),
                    Push = table.Column<float>(type: "real", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false),
                    SortID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortingElements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SortingElements_Elements_ElementID",
                        column: x => x.ElementID,
                        principalTable: "Elements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SortingElements_Sorts_SortID",
                        column: x => x.SortID,
                        principalTable: "Sorts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SortingElements_ElementID",
                table: "SortingElements",
                column: "ElementID");

            migrationBuilder.CreateIndex(
                name: "IX_SortingElements_SortID",
                table: "SortingElements",
                column: "SortID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_SortingElements_FirstElementID",
                table: "ElementComparisons",
                column: "FirstElementID",
                principalTable: "SortingElements",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_SortingElements_SecondElementID",
                table: "ElementComparisons",
                column: "SecondElementID",
                principalTable: "SortingElements",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementComparisons_Sorts_SortID",
                table: "ElementComparisons",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_ElementLists_ListID",
                table: "Elements",
                column: "ListID",
                principalTable: "ElementLists",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sorts_ElementLists_ElementListID",
                table: "Sorts",
                column: "ElementListID",
                principalTable: "ElementLists",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_ElementLists_ListID",
                table: "Tags",
                column: "ListID",
                principalTable: "ElementLists",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SortingElements_FirstElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_SortingElements_SecondElementID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementComparisons_Sorts_SortID",
                table: "ElementComparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_Elements_ElementLists_ListID",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Sorts_ElementLists_ElementListID",
                table: "Sorts");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ElementLists_ListID",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "SortingElements");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ListID",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ElementListID",
                table: "Sorts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Elements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ListID",
                table: "Elements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SortID",
                table: "ElementComparisons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SelectElements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElementID = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    LastTimeAsOption = table.Column<int>(type: "int", nullable: false),
                    NumberOfTimesAsOption = table.Column<int>(type: "int", nullable: false),
                    Push = table.Column<float>(type: "real", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false),
                    SortID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectElements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SelectElements_Elements_ElementID",
                        column: x => x.ElementID,
                        principalTable: "Elements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectElements_Sorts_SortID",
                        column: x => x.SortID,
                        principalTable: "Sorts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectElements_ElementID",
                table: "SelectElements",
                column: "ElementID");

            migrationBuilder.CreateIndex(
                name: "IX_SelectElements_SortID",
                table: "SelectElements",
                column: "SortID");

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
                name: "FK_ElementComparisons_Sorts_SortID",
                table: "ElementComparisons",
                column: "SortID",
                principalTable: "Sorts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_ElementLists_ListID",
                table: "Elements",
                column: "ListID",
                principalTable: "ElementLists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sorts_ElementLists_ElementListID",
                table: "Sorts",
                column: "ElementListID",
                principalTable: "ElementLists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_ElementLists_ListID",
                table: "Tags",
                column: "ListID",
                principalTable: "ElementLists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
