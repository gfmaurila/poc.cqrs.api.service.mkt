using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Cryptography;
using System.Text;

#nullable disable

namespace Mkt.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir dados mockados
            InsertMockData(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        private void InsertMockData(MigrationBuilder migrationBuilder)
        {
            var users = GetMockUsers();

            foreach (var user in users)
            {
                migrationBuilder.InsertData(
                    table: "Users",
                    columns: new[] { "FullName", "Email", "Phone", "BirthDate", "CreatedAt", "Active", "Password", "GenerateCodeReset", "Role" },
                    values: new object[] { user.FullName, user.Email, user.Phone, user.BirthDate, user.CreatedAt, user.Active, user.Password, user.GenerateCodeReset, user.Role }
                );
            }
        }

        private static string ComputeSha256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static List<UserMock> GetMockUsers()
        {
            var faker = new Faker("en");
            var users = new List<UserMock>();

            for (int i = 0; i < 100; i++) // Gerar 5 usuários fictícios
            {
                users.Add(new UserMock
                {
                    FullName = faker.Name.FullName(),
                    Email = faker.Internet.Email(),
                    Phone = faker.Phone.PhoneNumber(),
                    BirthDate = faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                    CreatedAt = DateTime.Now,
                    Active = faker.Random.Bool(),
                    Password = ComputeSha256Hash("Test@123"),
                    GenerateCodeReset = Guid.NewGuid().ToString(),
                    Role = "User"
                });
            }

            for (int i = 0; i < 100; i++) // Gerar 5 usuários fictícios
            {
                users.Add(new UserMock
                {
                    FullName = faker.Name.FullName(),
                    Email = faker.Internet.Email(),
                    Phone = faker.Phone.PhoneNumber(),
                    BirthDate = faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                    CreatedAt = DateTime.Now,
                    Active = faker.Random.Bool(),
                    Password = ComputeSha256Hash("Test@123"),
                    GenerateCodeReset = Guid.NewGuid().ToString(),
                    Role = "Admin"
                });
            }


            users.Add(new UserMock
            {
                FullName = "Guilherme F Maurila",
                Email = "gfmaurila@gmail.com",
                Phone = faker.Phone.PhoneNumber(),
                BirthDate = faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                CreatedAt = DateTime.Now,
                Active = faker.Random.Bool(),
                Password = ComputeSha256Hash("@G22u01i2025"),
                GenerateCodeReset = Guid.NewGuid().ToString(),
                Role = "Admin"
            });

            return users;
        }

        private class UserMock
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public DateTime BirthDate { get; set; }
            public DateTime CreatedAt { get; set; }
            public bool Active { get; set; }
            public string Password { get; set; }
            public string GenerateCodeReset { get; set; }
            public string Role { get; set; }
        }
    }
}
