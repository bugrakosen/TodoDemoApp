using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using TodoDemoApp.API.Data;
using TodoDemoApp.API.Data.Abstract;
using TodoDemoApp.API.Data.Concrete;
using TodoDemoApp.API.DTOs;
using TodoDemoApp.API.Entity;
using TodoDemoApp.API.Services.Abstract;
using TodoDemoApp.API.Services.Concrete;

namespace TodoDemoApp.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO null value check
            services.AddMvc().AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                opts.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;

            });

            Console.WriteLine($"------------------ {Configuration.GetConnectionString("MySqlConnection")} -----------------");

            services.AddDbContext<TodoAppDbContext>(opt =>
            {
                opt.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            });

            

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBaseService<TodoDTO>, TodoService>();
            services.AddScoped<IBaseService<CategoryDTO>, CategoryService>();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Demo Todo App",
                    Description = "Demo Todo App for Education",
                    TermsOfService = new Uri("https://milvasoft.com"),
                    Contact = new OpenApiContact { Name = "Milvasoft Yazýlým", Email = "info@milvasoft.com", Url = new Uri("https://milvasoft.com") },
                    License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Defult", "{controller=Todo}/{action=GetTodos}/{id?}");
            });

            #region Seed

            var dbContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<TodoAppDbContext>();

            dbContext.Database.MigrateAsync().Wait();

            var categories = dbContext.Set<Category>();

            int categoryCount = categories.CountAsync().Result;

            if (categoryCount == 0)
            {
                categories.Add(new Category { Name = "Yapacaklarým" });
                categories.Add(new Category { Name = "Bugün Yapacaklarým" });
            }

            dbContext.SaveChangesAsync().Wait();

            #endregion

            app.UseSwagger(conf =>
            {
                conf.RouteTemplate = "/docs/{documentName}/docs.json";
            }).UseSwaggerUI(conf =>
               {
                   conf.SwaggerEndpoint($"/docs/v1.0/docs.json", "Todo Demo App v1.0");
               });
        }
    }
}
