using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.MVC.Models
{
    public class Campaign
    {
        public int CampID { get; set; }                 //campaign ID

        public List<Character> Characters = new List<Character>();
        public List<User> GMs = new List<User>();

    }
}
