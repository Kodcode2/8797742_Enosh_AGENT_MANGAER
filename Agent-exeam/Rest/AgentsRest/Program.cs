
using AgentsRest.Middleware;
using AgentsRest.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserApi.Data;

namespace AgentsRest
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
            builder.Services.AddScoped<IAgentService, AgentService>();
            builder.Services.AddScoped<ITargetService, TargetService>();
            builder.Services.AddTransient(typeof(Lazy<>), typeof(Lazy<>));
            builder.Services.AddScoped<IMissionService, MissionService>();
			builder.Services.AddScoped<ILoginService, LoginService>();
			builder.Services.AddScoped<IJwtService, JwtService>();


			builder.Services.AddDbContext<ApplicationDbContext>();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		   .AddJwtBearer(options =>
		   {
			   options.TokenValidationParameters = new()
			   {
				   ValidateIssuer = true,
				   ValidateAudience = true,
				   ValidateIssuerSigningKey = true,
				   ValidIssuer = builder.Configuration["Jwt:Issuer"],
				   ValidAudience = builder.Configuration["Jwt:Audience"],
				   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
			   };
		   });
			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
           // app.UseMiddleware<LoginMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
