﻿using BookShopApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Contracts.v1.Requests
{
    public class UpdateBookRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Language Language { get; set; }

        public List<Category> BookCategories { get; set; }

        public List<Author> BookAuthors { get; set; }
    }
}
