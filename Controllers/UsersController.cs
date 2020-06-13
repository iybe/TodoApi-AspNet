using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TodoContext _context;

        public UsersController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> GetUser()
        {
            int id = IdUserAuthenticate();
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users
        //[HttpPut]
        //[Authorize]
        //public async Task<IActionResult> PutUser(User user)
        //{
        //    _context.Entry(user).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
            
        //    return NoContent();
        //}

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users
        //[HttpDelete]
        //[Authorize]
        //public async Task<ActionResult<User>> DeleteUser()
        //{
        //    int userId = IdUserAuthenticate();

        //    User user = _context.Users
        //        .Where(t =>t.Id == userId)
        //        .First();

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
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
