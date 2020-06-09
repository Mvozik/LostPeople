using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanko3.Models
{
    public class UserView
    {
        [Required(ErrorMessage ="Nie podano nazwy!")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Nie podano hasła!")]
        public string Password { get; set; }
    }
}
