
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
            builder.Services.AddSingleton<Repositories.IEVOwnerRepository, Repositories.EVOwnerRepository>();
            builder.Services.AddScoped<Services.IEVOwnerService, Services.EVOwnerService>();
            builder.Services.AddSingleton<Repositories.IChargingStationRepository, Repositories.ChargingStationRepository>();
            builder.Services.AddScoped<Services.IChargingStationService, Services.ChargingStationService>();
            builder.Services.AddSingleton<Repositories.IBookingRepository, Repositories.BookingRepository>();
            builder.Services.AddScoped<Services.BookingService>();

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

            // Seed default Backoffice user if none exists
            using (var scope = app.Services.CreateScope())
            {
                // Creates a scope and seeds a default admin user.
                var userRepo = scope.ServiceProvider.GetRequiredService<Repositories.IUserRepository>();
                var existingAdmin = userRepo.GetByEmailAsync("admin@evcs.local").GetAwaiter().GetResult();
                if (existingAdmin == null)
                {
                    var admin = new Models.User
                    {
                        Email = "admin@evcs.local",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin#12345"),
                        FullName = "Backoffice Admin",
                        Role = Models.UserRole.Backoffice,
                        IsActive = true
                    };
                    userRepo.CreateAsync(admin).GetAwaiter().GetResult();
                }
            }

            app.Run();
        }
    }
}
