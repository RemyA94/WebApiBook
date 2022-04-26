using Microsoft.AspNetCore.Mvc;
using WebApiBook.Context;
using WebApiBook.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace WebApiBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public AuthorController(ApplicationDBContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> Get()
        {
            try
            {
                return await context.Author
                                    .Include(x => x.Books).ToListAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "ObtenerAutor")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            try
            {
                var autor = await context.Author
                                         .Include(x => x.Books)
                                         .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (autor == null)
                {
                    return NotFound();
                }
                return autor;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Author autor)
        {
            try
            {
                await context.Author.AddAsync(autor);
                await context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Author autor, int id)
        {
            if (id != autor.Id)
            {
                return BadRequest();
            }
            try
            {
                context.Entry(autor).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> Delete(int id)
        {
            try
            {
                var autor = await context.Author
                                         .Where(x => x.Id == id).FirstOrDefaultAsync();

                if (autor == null)
                {
                    return NotFound();
                }
                context.Entry(autor).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                return autor;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
