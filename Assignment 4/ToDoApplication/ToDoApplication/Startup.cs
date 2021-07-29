using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ToDoApplication.Web.GlobalErrorHandling.LoggerService;
using ToDoApplication.Data.Data;
using ToDoApplication.Data.Interfaces;
using ToDoApplication.Data.Repositories;
using ToDoApplication.Filter;
using ToDoApplication.Services.Interfaces;
using ToDoApplication.Services.Services;
using ToDoApplication.Web.GlobalErrorHandling.Extensions;

namespace ToDoApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

            AppDbContext appDbContext = new AppDbContext(connectionString);
            if (!appDbContext.Database.CanConnect())
            {
                //Create new database and tables
                appDbContext.Database.Migrate();
                //Seed data with default user
                SeedData.SeedUserData(appDbContext);
            }

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoApplication", Version = "v1" });
                c.OperationFilter<UserHeaderFilter>();
            });

            services.AddSingleton(Configuration);

            services.AddSingleton<ILoggerManager, LoggerManager>();

            // Data repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IToDoListRepository, ToDoListRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            // Application services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IToDoListService, ToDoListService>();
            services.AddTransient<ITaskService, TaskService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoApplication v1"));
            }

            app.ConfigureExceptionHandler(logger);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
