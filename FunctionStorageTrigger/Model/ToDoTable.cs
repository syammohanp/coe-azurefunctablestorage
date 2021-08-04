using System;
using Microsoft.Azure.Cosmos.Table;

namespace FunctionStorageTrigger.Model
{
    public class TodoTable : TableEntity
    {
        public DateTime CreatedTime { get; set; }

        public string Title { get; set; }
    }

    public class Todo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public string Title { get; set; }
    }
}
