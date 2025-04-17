using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContactApp.Models
{
    public class t_User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int c_UserId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage ="Username is required")]
        public string c_UserName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Email is required")]
        public string c_Email { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        ErrorMessage = "Password must be at least 6 characters long, contain an uppercase letter, a digit, and a special character.")]
        public string c_Password { get; set; }   

        [StringLength(500)]
        [Required(ErrorMessage ="Address is required")]
        public string c_Address {get; set;}

        [StringLength(10)]
        [Required(ErrorMessage = "Mobile no is required")]
        public string c_Mobile {get; set;}

        [StringLength(10)]
        public string c_Gender {get; set;}

       
        public string ? c_Image {get; set;}

        public IFormFile ProfilePicture { get; set; }
    
    }
}