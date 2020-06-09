using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanko3.Models
{
    public class User:IdentityUser
    {
        public User()
        {

        }

        public virtual IList<Lost> Losts { get; set; }
        public Role role { get; set; }
        public enum Role { Admin, User }

    }
}
