using System.Collections.Generic;

namespace WebApiBook.Entities
{
    public class Author
    {
        public int Id { get; set; }
  
        public string Nombre { get; set; }
        public List<Book> Books { get; set; }
    }
}
