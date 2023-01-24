using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SailpointBackend.Migrations
{
    /// <inheritdoc />
    public partial class seedCities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var reader = new StreamReader(@"world-cities_csv.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var city = reader.ReadLine();
                    migrationBuilder.InsertData(
                     table: "Cities",
                     columns: new[] { "Name" },
                     values: new object[] { city });
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
