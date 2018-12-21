using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class Campaign
    {
        public int CampID { get; set; }                 //campaign ID
        public string Name { get; set; }

        public List<Character> Characters = new List<Character>();
        public List<User> GMs = new List<User>();

    }
}
