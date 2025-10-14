using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfResearch.Financial.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPropagatedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "prp_users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prp_users");
        }
    }
}
