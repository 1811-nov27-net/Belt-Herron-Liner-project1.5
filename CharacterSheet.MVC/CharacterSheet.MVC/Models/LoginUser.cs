using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterSheet.MVC.Models
{
    public class LoginUser
    {
        [Required]
        public string Username { get; set; }                //username
        [Required]
        [DataType(DataType.Password)]
        [StringLength(500,MinimumLength =25,ErrorMessage ="Password must be at least 25 characters long")]
        public string password { get; set; }                //password
        public int UserID { get; set; }                     //user ID
    }
}
