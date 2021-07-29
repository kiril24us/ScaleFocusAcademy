using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ProjectManagementApplication.Api.CustomPolicy;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Data.Repositories;
using ProjectManagementApplication.DTO.Requests.WorkLogRequests;
using ProjectManagementApplication.Services.Interfaces;
using ProjectManagementApplication.Services.Services;
using System;
using System.Collections.Generic;
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
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;               
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());                
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectManagementApplication", Version = "v1" });

                // Adds the authorize button in swagger UI 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                // Uses the token from the authorize input and sends it as a header
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("hh:mm")
                });
            });

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

            // EF Identity
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();

            services.AddSingleton(Configuration);

            services.AddSingleton<ILoggerManager, LoggerManager>();

            // Data repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IWorkLogRepository, WorkLogRepository>();

            // Application services
            services.AddTransient<IUserManager, IdentityServerUserManager>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IWorkLogService, WorkLogService>();

            // IdentityServer
            IIdentityServerBuilder builder = services.AddIdentityServer((options) =>
            {
                options.EmitStaticAudienceClaim = true;
            })
                                   .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
                                   .AddInMemoryClients(IdentityConfig.Clients);

            builder.AddDeveloperSigningCredential();
            builder.AddResourceOwnerValidator<PasswordValidator>();

            // Authentication
            // Adds the asp.net auth services
            services
                .AddAuthorization()
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                // Adds the JWT bearer token services that will authenticate each request based on the token in the Authorize header
                // and configures them to validate the token with the options
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "https://localhost:5001/resources";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly",
                     policy => policy.RequireRole("Admin"));

                options.AddPolicy("AdminOrManagerOnly",
                     policy => policy.RequireRole("Admin", "Manager"));                
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreatorOnly",
                                  policy => policy.Requirements.Add(new CreatorRequirement()));

                options.AddPolicy("CreatorOrTeamMemberOnly",
                                 policy => policy.Requirements.Add(new CreatorOrTeamMemberRequirement()));

                options.AddPolicy("AssigneeOnly",
                                policy => policy.Requirements.Add(new AssigneeRequirement()));
            });
            
            services.AddTransient<IAuthorizationHandler, CreatorHandler>();
            services.AddTransient<IAuthorizationHandler, CreatorOrTeamMemberHandler>();
            services.AddTransient<IAuthorizationHandler, AssigneeHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            //Adds the Identityserver Middleware that will handle 
            app.UseIdentityServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectManagementApplication v1"));
            }

            app.ConfigureExceptionHandler(logger);

            app.UseRouting();

            // Adds the auth middlewares
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
