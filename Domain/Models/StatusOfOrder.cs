using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class StatusOfOrder
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<Order> Orders { get; set; }

        public StatusOfOrder()
        {
            this.Orders = new List<Order>();
        }
    }
}
