using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class User
    {
        public string username { get; set; }                //username
        public string password { get; set; }                //password
        public int userID { get; set; }                     //user ID

        public List<Character> Characters = new List<Character>();
        public List<Campaign> MyCampaigns = new List<Campaign>();

    }
}
