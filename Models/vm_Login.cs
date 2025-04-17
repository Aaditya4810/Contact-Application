using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContactApp.Models
{
    public class vm_Login
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string c_Email { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Password is required.")]
        public string c_Password { get; set; }
    }
}