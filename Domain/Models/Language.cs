using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class Language
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<Book> Books { get; set; }

        public Language()
        {
            Books = new List<Book>();
        }
    }
}
