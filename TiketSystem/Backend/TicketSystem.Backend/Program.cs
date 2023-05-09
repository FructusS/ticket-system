using Microsoft.EntityFrameworkCore;
using TicketSystem.Backend.Hubs;
using TicketSystem.Backend.Mapping;
using TicketSystem.Database;

namespace TicketSystem.Backend
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
            builder.Services.AddDbContext<TicketSystemDbContext>(options => options.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddSignalR();
            var app = builder.Build();
          //  AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<TicketHub>("/ticketHub");
            app.Run();
        }
    }
}