using stringify_backend.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace stringify_backend
{
    public class Program
    {
        public static string ftpUrl = "";
        public static string ftpUserName = "";
        public static string ftpPassword = "";

        public static int SaltLength = 64;

        public static Dictionary<string, User> LoggedInUsers = new Dictionary<string, User>();

        public static string GenerateSalt()
        {
            Random random = new Random();
            string karakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string salt = "";
            for (int i = 0; i < SaltLength; i++)
            {
                salt += karakterek[random.Next(karakterek.Length)];
            }
            return salt;
        }

        public static string CreateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public static void Main(string[] args)
        {
            LoggedInUsers["token"] = new User { Id = 1, Jogosultsag = 9 };

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                    options.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });
            builder.Services.AddDbContext<StringifyDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(10, 5, 0))
                )
            );

            /*builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                    options.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());

            });*/


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowOrigin");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
