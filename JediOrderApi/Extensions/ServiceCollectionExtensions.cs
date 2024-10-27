using Core.Interfaces;
using Core.Models.Domain;
using Infrastructure.Data;
using Infrastructure.Repository;
using Infrastructure.Repository.Interfaces;
using JediOrderApi.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace TFGInfotecApi.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void GetServices(this IServiceCollection services)
		{
            services.AddScoped<IProductRepository, ProductsRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddCors();
            services.AddAuthorization();
            services.AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<JediOrderDbContext>();
            // services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddLogging(c => c.AddConsole());   
        }

        public static void AutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles));
        }

        //public static void IdentityUsersAndRoles(this IServiceCollection services)
        //{
        //    services.AddIdentityCore<IdentityUser>()
        //    .AddRoles<IdentityRole>()
        //    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("JediOrder")
        //    .AddEntityFrameworkStores<JediOrderAuthDbContext>()
        //    .AddDefaultTokenProviders();

        //    services.Configure<IdentityOptions>(options =>
        //    {
        //        // Password settings
        //        options.Password.RequireDigit = true;
        //        options.Password.RequireLowercase = true;
        //        options.Password.RequireUppercase = true;
        //        options.Password.RequireNonAlphanumeric = true;
        //        options.Password.RequiredLength = 8;
        //        options.Password.RequiredUniqueChars = 1;

        //        // Lockout settings
        //        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Lockout duration
        //        options.Lockout.MaxFailedAccessAttempts = 5; // Maximum number of failed attempts before lockout
        //        options.Lockout.AllowedForNewUsers = true; // Enable lockout for new users
        //    });
        //}

        public static void AddSwaggerGenWithOptions(this IServiceCollection services)
		{
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Jedi Order API", Version = "v1" });

                // Define the security scheme
                //options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = JwtBearerDefaults.AuthenticationScheme
                //});

                // Add security requirement
    //            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = JwtBearerDefaults.AuthenticationScheme
    //            }
    //        },
    //        new string[] {}
    //    }
    //});
            });
        }

		//public static void AddAuthenticationJwtBearer(this IServiceCollection services, IConfiguration configuration) 
		//{
  //          services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  //              .AddJwtBearer(options =>
  //              options.TokenValidationParameters = new TokenValidationParameters
  //              {
  //                  ValidateIssuer = true,
  //                  ValidateAudience = true,
  //                  ValidateLifetime = true,
  //                  ValidateIssuerSigningKey = true,
  //                  ValidIssuer = configuration["Jwt:Issuer"],
  //                  ValidAudience = configuration["Jwt:Audience"],
  //                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
  //              });
  //      }

		public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddDbContext<JediOrderDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("JediOrderConnectionString")));

            //services.AddDbContext<JediOrderAuthDbContext>(options =>
            //options.UseSqlServer(configuration.GetConnectionString("JediOrderAuthConnectionString"))); ;
		}

        public static async Task MapDatabaseContextAsync(this WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<JediOrderDbContext>();
                await context.Database.MigrateAsync();
                await JediOrderDbContextSeed.SeedDataAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
