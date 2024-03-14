using EduGate.APIs.Errors;
using EduGate.APIs.Extentions;
using EduGate.APIs.Middlewares;
using EduGate.Core.Entities.Identity;
using EduGate.Core.Services.Contract;
using EduGate.Repositroy.Identity;
using EduGate.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduGate.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            webApplicationBuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();


            //webApplicationBuilder.Services.AddDbContext<EduGateContext>(options =>
            //{
            //    options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            //});

            webApplicationBuilder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webApplicationBuilder.Services.AddIdentityServices(webApplicationBuilder.Configuration);

            webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                            .SelectMany(p => p.Value.Errors)
                                            .Select(E => E.ErrorMessage)
                                            .ToArray();

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            webApplicationBuilder.Services.AddHttpClient();
            #endregion

            var app = webApplicationBuilder.Build();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _IdentityDbContext = services.GetRequiredService<AppDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            try
            {
                await _IdentityDbContext.Database.MigrateAsync();   // Update Database

                var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppDbContextSeed.SeedUserAsync(_userManager);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An Error has been occured during apply the migrations");
            }

            #region Configure Kestrel Middlewares
            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionMiddleware>();
            
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "EduGate");
                option.RoutePrefix = String.Empty;
            });
            //}
            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseStaticFiles();

            app.UseHttpsRedirection();


            app.MapControllers(); 

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            app.Run();
        }
    }
}