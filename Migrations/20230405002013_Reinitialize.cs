using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sorting_App.Migrations
{
    /// <inheritdoc />
    public partial class Reinitialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElementList",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementList", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListID = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Elements_ElementList_ListID",
                        column: x => x.ListID,
                        principalTable: "ElementList",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sorts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElementListID = table.Column<int>(type: "int", nullable: false),
                    SelectionCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sorts_ElementList_ElementListID",
                        column: x => x.ElementListID,
                        principalTable: "ElementList",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tags_ElementList_ListID",
                        column: x => x.ListID,
                        principalTable: "ElementList",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectElement",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElementID = table.Column<int>(type: "int", nullable: false),
                    LastTimeAsOption = table.Column<int>(type: "int", nullable: false),
                    NumberOfTimesAsOption = table.Column<int>(type: "int", nullable: false),
                    Push = table.Column<float>(type: "real", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false),
                    SortID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectElement", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SelectElement_Elements_ElementID",
                        column: x => x.ElementID,
                        principalTable: "Elements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectElement_Sorts_SortID",
                        column: x => x.SortID,
                        principalTable: "Sorts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ElementElementTag",
                columns: table => new
                {
                    ElementsID = table.Column<int>(type: "int", nullable: false),
                    TagsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementElementTag", x => new { x.ElementsID, x.TagsID });
                    table.ForeignKey(
                        name: "FK_ElementElementTag_Elements_ElementsID",
                        column: x => x.ElementsID,
                        principalTable: "Elements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementElementTag_Tags_TagsID",
                        column: x => x.TagsID,
                        principalTable: "Tags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ElementComparison",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstElementID = table.Column<int>(type: "int", nullable: false),
                    SecondElementID = table.Column<int>(type: "int", nullable: false),
                    PreferenceDegree = table.Column<int>(type: "int", nullable: true),
                    SortID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementComparison", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ElementComparison_SelectElement_FirstElementID",
                        column: x => x.FirstElementID,
                        principalTable: "SelectElement",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ElementComparison_SelectElement_SecondElementID",
                        column: x => x.SecondElementID,
                        principalTable: "SelectElement",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ElementComparison_Sorts_SortID",
                        column: x => x.SortID,
                        principalTable: "Sorts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElementComparison_FirstElementID",
                table: "ElementComparison",
                column: "FirstElementID");

            migrationBuilder.CreateIndex(
                name: "IX_ElementComparison_SecondElementID",
                table: "ElementComparison",
                column: "SecondElementID");

            migrationBuilder.CreateIndex(
                name: "IX_ElementComparison_SortID",
                table: "ElementComparison",
                column: "SortID");

            migrationBuilder.CreateIndex(
                name: "IX_ElementElementTag_TagsID",
                table: "ElementElementTag",
                column: "TagsID");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ListID",
                table: "Elements",
                column: "ListID");

            migrationBuilder.CreateIndex(
                name: "IX_SelectElement_ElementID",
                table: "SelectElement",
                column: "ElementID");

            migrationBuilder.CreateIndex(
                name: "IX_SelectElement_SortID",
                table: "SelectElement",
                column: "SortID");

            migrationBuilder.CreateIndex(
                name: "IX_Sorts_ElementListID",
                table: "Sorts",
                column: "ElementListID");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ListID",
                table: "Tags",
                column: "ListID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElementComparison");

            migrationBuilder.DropTable(
                name: "ElementElementTag");

            migrationBuilder.DropTable(
                name: "SelectElement");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "Sorts");

            migrationBuilder.DropTable(
                name: "ElementList");
        }
    }
}
