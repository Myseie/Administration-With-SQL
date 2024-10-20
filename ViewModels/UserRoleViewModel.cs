using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Administration.ViewModels
{
    public class UserRolesViewModel
    {
        public IdentityUser User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}