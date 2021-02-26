using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TodoDemoApp.API.DTOs;
using TodoDemoApp.API.Services.Abstract;

namespace TodoDemoApp.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private IBaseService<TodoDTO> _todoService;


        public TodosController(IBaseService<TodoDTO> todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodosAsync()
        {
            var todos = await _todoService.GetEntitiesAsync().ConfigureAwait(false);
            return Ok(todos);
        }

        [HttpGet("Todo/{id}")]
        public async Task<IActionResult> GetTodoAsync(Guid id)
        {
            var todo = await _todoService.GetEntityAsync(id).ConfigureAwait(false);
            return Ok(todo);
        }

        [HttpPost("Todo")]
        public async Task<IActionResult> AddTodoAsync(TodoDTO todoDTO)
        {
            var todoId = await _todoService.AddEntityAsync(todoDTO).ConfigureAwait(false);

            if (todoId != Guid.Empty)
                return Ok(todoId);
            else return BadRequest("Bir hata oluştu lütfen tekrar deneyiniz.");
        }

        [HttpPut("Todo")]
        public async Task<IActionResult> UpdateTodoAsync(TodoDTO todoDTO)
        {
            await _todoService.UpdateEntityAsync(todoDTO).ConfigureAwait(false);
            return Ok("İşlem Başarılı");
        }

        [HttpDelete("Todo/{id}")]
        public async Task<IActionResult> DeleteTodoAsync(Guid id)
        {
            await _todoService.DeleteEntityAsync(id).ConfigureAwait(false);
            return Ok("İşlem Başarılı");
        }

    }
}
