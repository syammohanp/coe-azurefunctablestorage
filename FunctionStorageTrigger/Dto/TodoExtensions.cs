using FunctionStorageTrigger.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionStorageTrigger.Dto
{
    public static class TodoExtensions
    {
        public static TodoTable ToTable(this Todo todo)
        {
            return new TodoTable
            {
                PartitionKey = "TODO",
                RowKey = todo.Id,
                CreatedTime = todo.CreatedTime,
                Title = todo.Title
            };
        }

        public static Todo ToTodo(this TodoTable todoTable)
        {
            return new Todo
            {
                Id = todoTable.RowKey,
                CreatedTime = todoTable.CreatedTime,
                Title = todoTable.Title
            };
        }
    }
}
