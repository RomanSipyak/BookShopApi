using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int LanguageId { get; set; }

        public Language Language { get; set; }

        public List<BookCategory> BookCategories { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }

        public List<Unit> Units { get; set; }

        public Book()
        {
            BookCategories = new List<BookCategory>();

            BookAuthors = new List<BookAuthor>();

            Units = new List<Unit>();
        }
    }
}
