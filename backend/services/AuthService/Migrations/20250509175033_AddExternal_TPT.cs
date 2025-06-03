using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AddExternal_TPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalEvaluators",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Organisation = table.Column<string>(type: "text", nullable: false),
                    Domaine = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalEvaluators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalEvaluators_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalEvaluators");
        }
    }
}
