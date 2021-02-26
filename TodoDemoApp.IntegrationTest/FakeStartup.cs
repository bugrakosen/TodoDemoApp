using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TodoDemoApp.API;
using TodoDemoApp.API.Data;
using TodoDemoApp.API.Data.Abstract;
using TodoDemoApp.API.Data.Concrete;
using TodoDemoApp.API.DTOs;
using TodoDemoApp.API.Services.Abstract;
using TodoDemoApp.API.Services.Concrete;

namespace TodoDemoApp.IntegrationTest
{
    public class FakeStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                opts.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;

            });

            services.AddDbContext<TodoAppDbContext>(opt =>
            {
                opt.UseMySql("Server=localhost;Database=todoappdbTest;Uid=root;Pwd=root;");
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBaseService<TodoDTO>, TodoService>();
            services.AddScoped<IBaseService<CategoryDTO>, CategoryService>();

            services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Defult", "{controller=Todo}/{action=GetTodos}/{id?}");
            });
        }
    }
}
