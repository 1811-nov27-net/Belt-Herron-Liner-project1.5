using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    class Character
    {
        public int charID { get; set; }                     //character ID
        public int userID { get; set; }                     //ID of user the character belongs to
        public int CampID { get; set; }                     //ID of campaign the character belongs to
    }
}
