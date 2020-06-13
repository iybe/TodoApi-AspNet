using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using TodoApi.Models;

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
            var userId = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .First().ToString();
            int id = System.Convert.ToInt32(userId);

            var todosUser =  _context.TodoItems.Where(todo => todo.User_Id == id);
            
            if (todosUser.Any())
            {
                return await todosUser.ToListAsync();
            }
            return NotFound(new { Message = "Dados do usuario incorretos" });

        }

        // GET: api/TodoItems/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }*/

        // PUT: api/TodoItems/5
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/TodoItems
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody] TodoItem todoItem)
        {
            var newTodo = _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return newTodo.Entity;
            //return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        /*[HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }*/
    }
}
