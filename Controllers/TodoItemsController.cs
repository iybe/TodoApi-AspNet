using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using System.Security.Claims;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        /*  - objetivo da função: retornar os todos de um usuario
         *  - entrada de dados: por meio do campo body, enviar um json que descreva o objeto user
         */
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<dynamic>> GetTodoItems()
        {
            int userId = IdUserAuthenticate();

            var todosUser =  await _context.TodoItems
                .Where(todo => todo.User_Id == userId)
                .ToListAsync();

            return todosUser;
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<TodoItem> GetTodoItem(int id)
        {
            int userId = IdUserAuthenticate();

            try
            {
                var todoItem = _context.TodoItems
                    .Where(t => t.Id == id && t.User_Id == userId)
                    .First();
                return todoItem;
            }
            catch
            {
                return NotFound(new { message = "Todo não encontrado"});
            }
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            int userId = IdUserAuthenticate();
            TodoItem todo;

            try
            {
                todo = _context.TodoItems
                    .Where(t => t.Id == id && t.User_Id == userId)
                    .First();
            }
            catch
            {
                return NotFound(new { message = "Todo não encontrado" });
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/TodoItems
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody] TodoItem todoItem)
        {
            todoItem.User_Id = IdUserAuthenticate();
            var newTodo = _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return newTodo.Entity;
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
        {
            int userId = IdUserAuthenticate();

            var todoItem = _context.TodoItems
                .Where(t => t.User_Id == userId && t.Id == id)
                .First();

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        //metodo de servico, retorna o id do usuario autenticado
        private int IdUserAuthenticate()
        {
            var userId = User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .First().ToString();
            
            return System.Convert.ToInt32(userId);
        }
    }
}
