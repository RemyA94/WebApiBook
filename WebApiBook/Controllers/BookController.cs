using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBook.Entities;

namespace WebApiBook.Context
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public BookController(ApplicationDBContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            try
            {
                return await context.Book
                                    .Include(x => x.Author).ToListAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "ObtenerLibro")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            try
            {
                var libro = await context.Book
                                         .Include(x => x.Author)
                                         .Where(x => x.Id == id).FirstOrDefaultAsync();

                if (libro == null)
                {
                    return NotFound();
                }
                return libro;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Book libro)
        {
            try
            {
                await context.Book.AddAsync(libro);
                await context.SaveChangesAsync();
                return new CreatedAtRouteResult("ObtenerLibro", new { id = libro.Id }, libro);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Book libro, int id)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }
            try
            {
                context.Entry(libro).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> Delete(int id)
        {
            try
            {
                var libro = await context.Book
                                         .Where(x => x.Id == id).FirstOrDefaultAsync();

                if (libro == null)
                {
                    return NotFound();
                }
                context.Book.Remove(libro);
                await context.SaveChangesAsync();
                return libro;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}
