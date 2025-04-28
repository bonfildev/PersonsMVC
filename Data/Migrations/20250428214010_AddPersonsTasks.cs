using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonsMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonsTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RowItems",
                table: "RowItems");

            migrationBuilder.RenameTable(
                name: "RowItems",
                newName: "PersonsTasks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonsTasks",
                table: "PersonsTasks",
                column: "Idtask");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonsTasks",
                table: "PersonsTasks");

            migrationBuilder.RenameTable(
                name: "PersonsTasks",
                newName: "RowItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RowItems",
                table: "RowItems",
                column: "Idtask");
        }
    }
}
