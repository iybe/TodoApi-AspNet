using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using TodoApi.Services;
using System.Net.Http;

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
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = _context.Users
                .Where(u => u.Username == model.Username && u.Password == model.Password)
                .First();

            if (user == null)
                return NotFound(new { message = "Usuario ou senha incorretos" });

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                User = user,
                Token = token
            };
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "An�nimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() 
        {
            string resp = "";
            //foreach(var v in User.Identities)
            //{
            //    resp += v.Claims + " = " + v.Name + "\n";
            //}
            resp = User
                .Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .First()
                .ToString();

            //return String.Format("Autenticado - {0}", User.Claims);
            return resp;
        }

    }
}