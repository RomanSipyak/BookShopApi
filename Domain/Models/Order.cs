using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int StatusOfOrderId { get; set; }

        public int TotalPrice { get; set; }

        public StatusOfOrder StatusOfOrder { get; set; }

        public List<Unit> Units { get; set; }

        public Order()
        {
            this.Units = new List<Unit>();
        }
    }
}
