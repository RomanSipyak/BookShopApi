using BookShopApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Contracts.v1.Requests
{
    public class CreateBookRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Language Language { get; set; }

        public List<Category> BookCategories { get; set; }

        public List<Author> BookAuthors { get; set; }

        //public List<Unit> Units { get; set; }
    }
}
