using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterSheet.MVC.Models
{
    public class User
    {
        [Required]
        public string username { get; set; }                //username
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }                //password
        public int userID { get; set; }                     //user ID

        public List<Character> Characters = new List<Character>();
        public List<Campaign> MyCampaigns = new List<Campaign>();

    }
}
