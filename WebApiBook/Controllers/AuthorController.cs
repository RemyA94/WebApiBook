using Microsoft.AspNetCore.Mvc;
using WebApiBook.Context;
using WebApiBook.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApiBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public AuthorController(ApplicationDBContext context )
        {
            
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Author>> Get()
        {
            return context.Author.Include(x => x.Books).ToList();
        }

        [HttpGet("{id}", Name = "ObtenerAutor")]
        public ActionResult<Author> Get(int id)
        {
            var autor = context.Author.Include(x => x.Books).FirstOrDefault(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Author autor)
        {
            context.Author.Add(autor);
            context.SaveChanges();

            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor);
        }

        [HttpPut("{id}")]
        public ActionResult Put([FromBody] Author autor, int id)
        {
            if (id != autor.Id)
            {
                return BadRequest();
            }

            context.Entry(autor).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Author> Delete(int id)
        {
            var autor = context.Author.FirstOrDefault(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }          
            context.Entry(autor).State = EntityState.Deleted;
            context.SaveChanges();
            return autor;
        }
    }
}

