
namespace EVChargingWebService
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

            // Configure options and dependencies
            // - MongoDB settings
            builder.Services.Configure<Config.MongoDbSettings>(
                builder.Configuration.GetSection("MongoDb"));

            // - JWT settings
            builder.Services.Configure<Config.JwtSettings>(
                builder.Configuration.GetSection("JwtSettings"));

            // - Repositories and Services
            builder.Services.AddSingleton<Repositories.IUserRepository, Repositories.UserRepository>();
            builder.Services.AddScoped<Services.IUserService, Services.UserService>();

            // - Authentication/Authorization
            var jwtSettings = new Config.JwtSettings();
            builder.Configuration.Bind("JwtSettings", jwtSettings);
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes)
                    };
                });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
