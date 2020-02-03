using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Contracts.v1.Responses
{
    public class UserResponse
    {
        public string Email { get; set; }

        public List<Role> userRoles { get; set; }
    }

    public class Role
    {
        public string Title { get; set; }
    }
}
