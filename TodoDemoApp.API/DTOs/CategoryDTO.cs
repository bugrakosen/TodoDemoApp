using System.Collections.Generic;
using TodoDemoApp.API.Entity;

namespace TodoDemoApp.API.DTOs
{
    public class CategoryDTO : BaseEntity
    {
        public string Name { get; set; }
        public int TodoCount { get; set; }

        public List<TodoDTO> Todos { get; set; }
    }
}
