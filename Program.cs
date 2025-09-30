
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

            // CORS: Allow frontend dev origins and auth headers
            const string CorsPolicyName = "FrontendCors";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicyName, policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:5173",
                            "http://127.0.0.1:5173",
                            "https://localhost:5173",
                            "https://127.0.0.1:5173"
                        )
                        .AllowAnyMethod()
                        .WithHeaders("Content-Type", "Authorization");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Avoid HTTPS redirection in development to prevent cert/redirect issues with frontend
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            // Must be before authentication/authorization
            app.UseCors(CorsPolicyName);

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
