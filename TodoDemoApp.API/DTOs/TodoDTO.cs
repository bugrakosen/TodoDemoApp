using System;
using TodoDemoApp.API.Entity;
using TodoDemoApp.API.Utils;

namespace TodoDemoApp.API.DTOs
{
    public class TodoDTO : BaseEntity
    {
        /// <summary>
        /// User defined task content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Remind date of task.
        /// </summary>
        public DateTime? ReminMeDate { get; set; }

        /// <summary>
        /// Due date of task.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Foreing key of category.
        /// </summary>
        [ValidateId]
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        /// <summary>
        /// Navigation property of category.
        /// </summary>
        public virtual CategoryDTO Category { get; set; }
    }
}
