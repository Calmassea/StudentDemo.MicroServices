using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentDemo.HubWebapi.Migrations
{
    public partial class AddStuData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Profession = table.Column<string>(nullable: true),
                    SClass = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "students",
                columns: new[] { "ID", "Age", "Name", "Profession", "SClass" },
                values: new object[,]
                {
                    { 1, 20, "黎明", "软件工程", "软件1907" },
                    { 2, 21, "小红", "软件工程", "软件1807" },
                    { 3, 19, "黎明", "软件工程", "软件1907" },
                    { 4, 20, "小明", "软件工程", "软件1907" },
                    { 5, 22, "小黑", "网络工程", "软件1907" },
                    { 6, 18, "黎明", "网络工程", "软件1907" },
                    { 7, 21, "张三", "网络工程", "软件1707" },
                    { 8, 21, "黎明", "软件工程", "软件1907" },
                    { 9, 24, "李四", "软件工程", "软件1907" },
                    { 10, 15, "王五", "软件工程", "软件1907" },
                    { 11, 20, "孙八", "软件工程", "软件1907" },
                    { 12, 20, "小六", "软件工程", "软件1907" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "students");
        }
    }
}
