using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<BookCategory> BookCategories { get; set; }

        public Category()
        {
            this.BookCategories = new List<BookCategory>();
        }
    }
}
