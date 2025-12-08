
using Core_WebAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Text;

namespace Core_WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });


            // Configure EF Core with SQL Server
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            //);

            try
            {
                var connStr = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
                using var conn = new MySqlConnection(connStr);
                conn.Open();
                Console.WriteLine("✅ Successfully connected to Railway MySQL!");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ MySQL connection failed: " + ex.Message);
            }

            // JWT Authentication
            var key = Encoding.ASCII.GetBytes("YourSuperSecretKey123!"); // store in appsettings.json for production
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            var app = builder.Build();
            app.UseCors("AllowReact");


            // Configure the HTTP request pipeline.
            
                app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger"; // optional: keeps swagger at /swagger
            });


            //app.UseHttpsRedirection();
            app.UseAuthentication(); // ✅ Important: Authentication middleware
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
