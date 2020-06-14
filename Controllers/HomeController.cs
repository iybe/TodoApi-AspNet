using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using TodoApi.Services;
using TodoApi.DTOs;

namespace TodoApi.Controllers
{
    [Route("api")]
    public class HomeController : ControllerBase
    {
        private readonly TodoContext _context;

        public HomeController(TodoContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] UserDTO model)
        {
            User user;
            try
            {
                user = _context.Users
                    .Where(u => u.Username == model.Username && u.Password == model.Password)
                    .First();
            }
            catch
            {
                return NotFound(new { message = "Usuario ou senha incorretos" });
            }

            var token = TokenService.GenerateToken(user);

            return new
            {
                User = model,
                Token = token
            };
        }

    }
}