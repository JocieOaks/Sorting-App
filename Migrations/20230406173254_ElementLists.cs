using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sorting_App.Migrations
{
    /// <inheritdoc />
    public partial class ElementLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_ElementList_ListID",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Sorts_ElementList_ElementListID",
                table: "Sorts");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ElementList_ListID",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElementList",
                table: "ElementList");

            migrationBuilder.RenameTable(
                name: "ElementList",
                newName: "ElementLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElementLists",
                table: "ElementLists",
                column: "ID");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_ElementLists_ListID",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Sorts_ElementLists_ElementListID",
                table: "Sorts");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ElementLists_ListID",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElementLists",
                table: "ElementLists");

            migrationBuilder.RenameTable(
                name: "ElementLists",
                newName: "ElementList");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElementList",
                table: "ElementList",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_ElementList_ListID",
                table: "Elements",
                column: "ListID",
                principalTable: "ElementList",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sorts_ElementList_ElementListID",
                table: "Sorts",
                column: "ElementListID",
                principalTable: "ElementList",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_ElementList_ListID",
                table: "Tags",
                column: "ListID",
                principalTable: "ElementList",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
