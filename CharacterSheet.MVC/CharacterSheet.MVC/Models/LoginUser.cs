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
        public string password { get; set; }                //password
        public int UserID { get; set; }                     //user ID
    }
}
