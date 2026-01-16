using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtAuth.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRole : Migration
    {
        /// <inheritdoc />
        // protected override void Up(MigrationBuilder migrationBuilder)
        // {
        //     migrationBuilder.AddColumn<string>(
        //         name: "role",
        //         table: "Users",
        //         type: "nvarchar(max)",
        //         nullable: false,
        //         defaultValue: "");
        // }

        // /// <inheritdoc />
        // protected override void Down(MigrationBuilder migrationBuilder)
        // {
        //     migrationBuilder.DropColumn(
        //         name: "role",
        //         table: "Users");
        // }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update all users where role is currently empty
            migrationBuilder.Sql(@"
                UPDATE [Users]
                SET [role] = 'Admin'
                WHERE [role] = ''
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert it back to empty if rolled back
            migrationBuilder.Sql(@"
                UPDATE [Users]
                SET [role] = ''
                WHERE [role] = 'Admin'
            ");
        }
    }
}
