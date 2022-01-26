using AutoMapper;
using E_BOOKLIBRARY.Data.Db;
using E_BOOKLIBRARY.Data.Repositories.BookRepo;
using E_BOOKLIBRARY.Data.Repositories.CategoryRepo;
using E_BOOKLIBRARY.Extensions;
using E_BOOKLIBRARY.Models;
using E_BOOKLIBRARY.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY
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
            services.AddControllers();
            services.AddDbContextPool<BookLibraryContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<AppUser, IdentityRole>(
            //    options =>
            //{
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 5;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequiredUniqueChars = 0;
            //}
                ).AddEntityFrameworkStores<BookLibraryContext>();
            services.AddMemoryCache();
            services.AddScoped<Seeder>();
            services.AddAutoMapper();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookRepo, BookRepo>();
            services.AddScoped<IBookService, BookService>();
            services.ConfigureCors();
            services.ConfigureIISIntegration();




            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-BOOKLIBRARY", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Authentication and Authorization
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            Seeder.SeedMe(app);
            Seeder.SeedUsersAndRolesAsync(app).Wait();
            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "WalkingSkeleton-v1"));
        }
    }
}
