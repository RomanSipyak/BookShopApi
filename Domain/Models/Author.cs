using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }

        public Author()
        {
            this.BookAuthors = new List<BookAuthor>();
        }
    }
}
