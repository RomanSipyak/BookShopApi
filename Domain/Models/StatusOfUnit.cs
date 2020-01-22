using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Domain.Models
{
    public class StatusOfUnit
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public List<Unit> Units { get; set; }

        public StatusOfUnit()
        {
            Units = new List<Unit>();

        }
    }
}
