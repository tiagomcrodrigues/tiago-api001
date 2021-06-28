using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api001.Data;
using api001.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using api001.Services;

namespace api001.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {

            var pruducts = await context
                .Users
                .AsNoTracking()
                .ToListAsync();
            return Ok(User);
        }

        [HttpPost("")]
        [AllowAnonymous]
        //[Authorize(Roles = "manager")]
        public async Task<IActionResult> Post
         (
             [FromServices] DataContext context,
              [FromBody] User model
         )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            try
            {
                model.Role = "Employee";
                context.Users.Add(model);
                await context.SaveChangesAsync();
                
                model.Password = "";

                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel criar a categoria" });
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate
            ([FromServices] DataContext context,
             [FromBody] User model)
        {
            var user = await context.Users
                 .AsNoTracking()
                 .Where(x => x.Username == model.Username && x.Password == model.Password)
                 .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "Usuario ou senha invàlido" });
            }
            else
            {
                var token = tokenService.GenerateToken(user);
                user.Password = "";
                return new
                {
                    user = user,
                    token = token
                };
            }
        }

        [HttpPut("{Id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put
            (
            int id,
            [FromBody] User model,
            [FromServices] DataContext context
            )
        {


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar a categoria" });
            }



        }


    }
}
