using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        //[Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        public int LanguageId { get; set; }

        public Language Language { get; set; }

        public List<BookCategory> BookCategories { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }

        public List<Unit> Units { get; set; }

        public Book()
        {
            this.BookCategories = new List<BookCategory>();

            this.BookAuthors = new List<BookAuthor>();

            this.Units = new List<Unit>();
        }
    }
}
