using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularAuthInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Users",
                newName: "ResetPasswordCode");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "Users",
                newName: "IX_Users_UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "ResetPasswordCode",
                table: "Users",
                newName: "Token");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserName",
                table: "Users",
                newName: "IX_Users_Username");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
