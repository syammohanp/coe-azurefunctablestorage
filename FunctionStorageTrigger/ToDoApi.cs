using FunctionStorageTrigger.Dto;
using FunctionStorageTrigger.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionStorageTrigger
{
    public static class ToDoApi
    {
        [FunctionName("Create")]
        public static async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "todo")] HttpRequest req,
        [Table("todos", Connection = "AzureWebJobsStorage")] IAsyncCollector<TodoTable> todoTableCollector,
        ILogger log)
        {
            log.LogInformation("Adding new Todo");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateTodoDto>(requestBody);

            if (input.Title == null)
            {
                return new BadRequestObjectResult("Please provide Title");
            }

            var todo = new Todo
            {
                Title = input.Title
            };

            await todoTableCollector.AddAsync(todo.ToTable());

            return new OkObjectResult(todo);
        }

        [FunctionName("GetAll")]
        public static async Task<IActionResult> GetAll(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo")] HttpRequest req,
        [Table("todos", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
        ILogger log)
        {
            log.LogInformation("Getting All ToDos");

            TableQuery<TodoTable> query = new TableQuery<TodoTable>();
            var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            var data = segment.Select(TodoExtensions.ToTodo);

            return new OkObjectResult(data);
        }
    }
}
