using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfResearch.UserManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class UserStateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "state",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "state",
                table: "users");
        }
    }
}
