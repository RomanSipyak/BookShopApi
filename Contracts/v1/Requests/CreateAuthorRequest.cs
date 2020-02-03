using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Contracts.v1.Requests
{
    public class CreateAuthorRequest
    {
        public string FullName { get; set; }

        public string Biography { get; set; }
    }
}
