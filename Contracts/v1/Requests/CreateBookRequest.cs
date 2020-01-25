using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Contracts.v1.Requests
{
    public class CreateBookRequest
    {

        public string Description { get; set; }

        public string Language { get; set; }

        public List<int> BookCategories { get; set; }

        public List<int> BookAuthors { get; set; }

        //public List<Unit> Units { get; set; }
    }
}
