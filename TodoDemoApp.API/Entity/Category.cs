using System.Collections.Generic;

namespace TodoDemoApp.API.Entity
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public List<Todo> Todos { get; set; }
    }
}
