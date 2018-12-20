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
        public int MaxHitPoints { get; set; }

        public Dictionary<string, int> SavingThrows { get; set; }
        public Dictionary<string, int> ClassLevels { get; set; }
        public Dictionary<string, int> Attributes { get; set; }
        public Dictionary<string, int> SkillList { get; set; }
        public Dictionary<string, int> FeatList { get; set; }
        public Dictionary<string, int> Invantory { get; set; }
        public Dictionary<string, int[]> SpellSlots { get; set; }
        public List<Spell> SpellsKnown { get; set; }

    }
}
