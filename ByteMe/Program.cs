
using ByteMe.API.Repositories.Interfaces;
using ByteMe.API.Repositories;
using ByteMe.API.Services.Interfaces;
using ByteMe.API.Services;
using ByteMe.API.Data;
using Microsoft.EntityFrameworkCore;
using ByteMe.API.Hubs;
using System;

namespace ByteMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(connectionString)
            );

            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddControllers();
            builder.Services.AddSignalR(); // Important for GameHub
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting(); // <-- Correct routing setup

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("/gameHub");
            });

            app.Run();

        }
    }
}
