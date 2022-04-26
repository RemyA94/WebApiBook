using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApiBook.Entities;

namespace WebApiBook.Context
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public BookController(ApplicationDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            var libros = context.Book.Include(x => x.Author).ToList();
            return libros;
        }

        [HttpGet("{id}", Name = "ObtenerLibro")]
        public ActionResult<Book> Get(int id)
        {
            var libro = context.Book.Include(x => x.Author).FirstOrDefault(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Book libro)
        {
            context.Book.Add(libro);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerLibro", new { id = libro.Id }, libro);
        }

        [HttpPut("{id}")]
        public ActionResult Put([FromBody] Book libro, int id)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }
            context.Entry(libro).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Book> Delete(int id)
        {
            var libro = context.Book.FirstOrDefault(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }
            context.Book.Remove(libro);
            context.SaveChanges();
            return libro;
        }

    }
}
   