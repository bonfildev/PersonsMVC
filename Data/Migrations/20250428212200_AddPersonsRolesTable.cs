using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonsMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonsRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop PK first
            migrationBuilder.DropPrimaryKey(
                name: "PK_RowItems",
                table: "RowItems");

            migrationBuilder.AlterColumn<long>(
                name: "Idtask",
                table: "RowItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
            // 3. Re-add PK
            migrationBuilder.AddPrimaryKey(
                name: "PK_RowItems",
                table: "RowItems",
                column: "Idtask");
            // 2. Create the new table
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IDRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IDRole);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Roles");

            // 1. Drop PK first
            migrationBuilder.DropPrimaryKey(
                name: "PK_RowItems",
                table: "RowItems");

            // 2. Revert column type
            migrationBuilder.AlterColumn<int>(
                name: "Idtask",
                table: "RowItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            // 3. Re-add PK
            migrationBuilder.AddPrimaryKey(
                name: "PK_RowItems",
                table: "RowItems",
                column: "Idtask");
        }
    }
}
