using Microsoft.AspNetCore.Mvc;
using CommonObjects.Models;
using TaskAPI.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var returnedTable = await _context.TaskTable.ToListAsync();
            return Ok(returnedTable);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<TaskObject>> GetById(int id)
        //{
        //    var returnedTaskObject = await _context.TaskTable.FindAsync(id);

        //    if (returnedTaskObject == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(returnedTaskObject);
        //}

        [HttpGet("{date}")]
        public async Task<ActionResult<TaskObject>> GetByDate(DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            var returned = await _context.TaskTable
                .Where(t => t.due_date >= start && t.due_date < end)
                .Where(s => s.completed == false)
                .ToListAsync();

            if (returned == null)
            {
                return NotFound();
            }

            return Ok(returned);
        }


        [HttpGet("count-by-type")]
        public ActionResult<Dictionary<string, int>> GetTaskCountByType()
        {
            var counts = _context.TaskTable
                .Select(t => t.type)
                .GroupBy(type => type)
                .ToDictionary(g => g.Key, g => g.Count());

            return Ok(counts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskObject taskObject)
        {
            if (taskObject == null)
            {
                return BadRequest("Task is missing.");
            }

            _context.TaskTable.Add(taskObject);
            await _context.SaveChangesAsync();

            return Ok(taskObject);
        }


        [HttpPatch]
        public async Task<IActionResult> Update(TaskObject taskObject)
        {
            var existingTask = await _context.TaskTable.FindAsync(taskObject.id);
            if (existingTask == null)
            {
                return NotFound("Task not found.");
            }
            if (taskObject.name != null)
            {
                existingTask.name = taskObject.name;
            }
            if (taskObject.description != null)
            {
                existingTask.description = taskObject.description;
            }
            if (taskObject.type != null)
            {
                existingTask.type = taskObject.type;
            }
            if (taskObject.due_date.HasValue)
            {
                existingTask.due_date = taskObject.due_date;
            }
            existingTask.completed = taskObject.completed;
            await _context.SaveChangesAsync();
            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingTask = await _context.TaskTable.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound("Task not found.");
            }

            _context.TaskTable.Remove(existingTask);
            await _context.SaveChangesAsync();
            return Ok("Task deleted successfully.");
        }
    }
}
