using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class Unit
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public int StatusOfUnitId { get; set; }

        public StatusOfUnit StatusOfUnit { get; set; }

        public int? OrderId { get; set; }

        public Order Order { get; set; }
    }
}
