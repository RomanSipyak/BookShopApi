using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Contracts.v1.Requests
{
    public class UpdateUserRequest
    {
        public string Email { get; set; }

        public List<RoleForUpdate> userRoles { get; set; }
    }

    public class RoleForUpdate
    {
        public string Title { get; set; }
    }
}
