using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ProjectManagementApplication.Api.Filter;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Data.Repositories;
using ProjectManagementApplication.DTO.Requests.WorkLogRequests;
using ProjectManagementApplication.Services.Auth;
using ProjectManagementApplication.Services.Interfaces;
using ProjectManagementApplication.Services.Services;
using System;
using ToDoApplication.Web.GlobalErrorHandling.Extensions;
using ToDoApplication.Web.GlobalErrorHandling.LoggerService;

namespace ProjectManagementApplication.Api
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
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            AppDbContext appDbContext = new AppDbContext(connectionString);
            if (!appDbContext.Database.CanConnect())
            {
                //Create new database and tables
                appDbContext.Database.Migrate();
                //Seed data with default user
                SeedData.SeedUserData(appDbContext);
            }

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectManagementApplication", Version = "v1" });
                c.OperationFilter<UserHeaderFilter>();
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("hh:mm")
                });
            });

            services.AddSingleton(Configuration);

            services.AddSingleton<ILoggerManager, LoggerManager>();

            // Data repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IWorkLogRepository, WorkLogRepository>();

            // Application services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IAuthentication, Authentication>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IWorkLogService, WorkLogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectManagementApplication v1"));
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
