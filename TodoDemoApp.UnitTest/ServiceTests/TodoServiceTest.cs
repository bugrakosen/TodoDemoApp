using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TodoDemoApp.API.Data;
using TodoDemoApp.API.Data.Abstract;
using TodoDemoApp.API.Data.Concrete;
using TodoDemoApp.API.DTOs;
using TodoDemoApp.API.Seed;
using TodoDemoApp.API.Services.Abstract;
using TodoDemoApp.API.Services.Concrete;
using Xunit;

namespace TodoDemoApp.UnitTest.ServiceTests
{
    public class TodoServiceTest
    {
        private readonly IBaseService<TodoDTO> _todoService;

        public TodoServiceTest()
        {
            _todoService = GetTodoService();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEntitiesAsync_IsComingAllDatas_AllDatasComing()
        {
            await SeedData.ResetDatas().ConfigureAwait(false);

            var actual = await _todoService.GetEntitiesAsync().ConfigureAwait(false);
            var expected = 10;

            Assert.Equal(expected, actual.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task MarkAsIsFavoriteAsync_MarkAsFavorite_MarkAsFavorite()
        {
            await SeedData.ResetDatas().ConfigureAwait(false);

            var entityId = 1.ToGuid();

            await _todoService.MarkAsIsFavoriteAsync(entityId, false).ConfigureAwait(false);

            var actual = await _todoService.GetEntityAsync(entityId).ConfigureAwait(false);
            var expected = false;

            Assert.Equal(expected, actual.IsFavorite);
        }

        private IBaseService<TodoDTO> GetTodoService()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBaseService<TodoDTO>, TodoService>();
            services.AddDbContext<TodoAppDbContext>(p=>p.UseMySql("Server=localhost;Database=todoappdbTest;Uid=root;Pwd=root;"));

            return services.BuildServiceProvider().GetRequiredService<IBaseService<TodoDTO>>();
        }
    }
}
