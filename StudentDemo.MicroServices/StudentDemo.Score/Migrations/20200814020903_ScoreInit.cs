using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentDemo.ScoreWebapi.Migrations
{
    public partial class ScoreInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    Chinese = table.Column<int>(nullable: false),
                    Maths = table.Column<int>(nullable: false),
                    English = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scores", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "scores",
                columns: new[] { "Id", "Chinese", "English", "Maths", "StudentId" },
                values: new object[,]
                {
                    { 1, 126, 41, 58, 1 },
                    { 2, 121, 81, 109, 2 },
                    { 3, 136, 150, 108, 3 },
                    { 4, 77, 79, 52, 4 },
                    { 5, 97, 132, 21, 5 },
                    { 6, 108, 63, 132, 6 },
                    { 7, 53, 120, 76, 7 },
                    { 8, 142, 26, 29, 8 },
                    { 9, 132, 23, 75, 9 },
                    { 10, 54, 43, 63, 10 },
                    { 11, 103, 114, 55, 11 },
                    { 12, 88, 31, 87, 12 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scores");
        }
    }
}
