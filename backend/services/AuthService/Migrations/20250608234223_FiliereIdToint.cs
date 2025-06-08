using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class FiliereIdToint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Etudiants\" ALTER COLUMN \"FiliereId\" TYPE integer USING \"FiliereId\"::integer;");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Etudiants\" ALTER COLUMN \"FiliereId\" TYPE text USING \"FiliereId\"::text;");
        }

    }
}
