using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesApi.Migrations
{
    /// <inheritdoc />
    public partial class updatetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_CreatedByUserIdId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_CreatedByUserIdId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserIdId",
                table: "Expenses",
                newName: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Expenses",
                newName: "CreatedByUserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CreatedByUserIdId",
                table: "Expenses",
                column: "CreatedByUserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_CreatedByUserIdId",
                table: "Expenses",
                column: "CreatedByUserIdId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
