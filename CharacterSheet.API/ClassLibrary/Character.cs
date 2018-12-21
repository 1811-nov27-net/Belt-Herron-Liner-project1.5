using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class Character
    {
        public int charID { get; set; }                     //character ID
        public int userID { get; set; }                     //ID of user the character belongs to
        public int CampID { get; set; }                     //ID of campaign the character belongs to
        public int MaxHitPoints { get; set; }

        public Dictionary<string, int> SavingThrows { get; set; }       // Saving Throw Type, Saving Throw Bonus
        public Dictionary<string, int> ClassLevels { get; set; }        // Class name, number of levels in class
        public Dictionary<string, int> Attributes { get; set; }         // Attribute name, level in attribute
        public Dictionary<string, int> SkillList { get; set; }          // Skill name, points in skill
        public Dictionary<string, int> FeatList { get; set; }           // Feat Name, number of times taken feat
        public Dictionary<string, int> Inventory { get; set; }          // Item name, number of such item
        public Dictionary<string, int[]> SpellSlots { get; set; }       // Class name, 10-element array repersenting spell levels 0-9, number of spell slots per level
        public List<Spell> SpellsKnown { get; set; }                    // list of spells known.

    }
}
