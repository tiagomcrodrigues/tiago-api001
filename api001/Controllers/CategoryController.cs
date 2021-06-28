using api001.Data;
using api001.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace api001.Controllers
{
    [Route("v1/categories")]
    [ApiController]
    [AllowAnonymous]
    public class CategoryController : ControllerBase
    {

        [HttpGet("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent" , Location = ResponseCacheLocation.Any, Duration = 30)]
        public async Task<IActionResult> Get([FromServices] DataContext context)   
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByid(
            int id,
            [FromServices] DataContext context
            )
        {
            var categories = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(categories);
        }


        [HttpPost("")]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> Post
        (
            [FromBody] Category model,
            [FromServices] DataContext context   
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel criar a categoria" });
            }

        }

        [HttpPut("{Id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> Put
            (
            int id ,
            [FromBody]Category model,
            [FromServices] DataContext context
            )
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar a categoria" });
            }



        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> Delete(
            int id,
            [FromServices]DataContext context
            )
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { massage = "Categoria não encotrada" });
            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso" });
            }
            catch(Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover a categoria" });
            }



        }

        [HttpPatch("")]
        [Authorize(Roles = "employee")]
        public string Patch()
        {
            return "Olá Patch!";
        }


    }
}
