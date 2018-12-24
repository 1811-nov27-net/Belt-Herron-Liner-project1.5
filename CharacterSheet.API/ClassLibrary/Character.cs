using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class Character
    {
        public Character()
        {
            AC = new int[3];
            EffectiveSavingThrows = new Dictionary<string, int>();
            BaseSavingThrows = new Dictionary<string, int>();
            ClassLevels = new Dictionary<string, int>();
            Attributes = new Dictionary<string, int>();
            AttributeBonuses = new Dictionary<string, int>();
            SkillList = new Dictionary<string, int>();
            FeatList = new Dictionary<string, int>();
            Inventory = new Dictionary<string, int>();
            SpellSlots = new Dictionary<string, int[]>();
            SpellsKnown = new List<Spell>();
        }


        public int CharID { get; set; }                     //character ID
        public int UserID { get; set; }                     //ID of user the character belongs to
        public int CampID { get; set; }                     //ID of campaign the character belongs to
        public int MaxHitPoints { get; set; }
        public string Race { get; set; }
        public string Sex { get; set; }
        public int BaseAttackBonus { get; set; }
        public int[] AC { get; set; }
        public string Alignment { get; set; }


        public Dictionary<string, int> EffectiveSavingThrows { get; set; }       // Saving Throw Type, Saving Throw Bonus

        public Dictionary<string, int> BaseSavingThrows { get; set; }       // Saving Throw Type, Saving Throw Bonus
        public Dictionary<string, int> ClassLevels { get; set; }        // Class name, number of levels in class
        public Dictionary<string, int> Attributes { get; set; }         // Attribute name, level in attribute
        public Dictionary<string, int> AttributeBonuses { get; set; }         // Attribute name, bonus from attribute
        public Dictionary<string, int> SkillList { get; set; }          // Skill name, points in skill
        public Dictionary<string, int> FeatList { get; set; }           // Feat Name, number of times taken feat
        public Dictionary<string, int> Inventory { get; set; }          // Item name, number of such item
        public Dictionary<string, int[]> SpellSlots { get; set; }       // Class name, 10-element array repersenting spell levels 0-9, number of spell slots per level
        public List<Spell> SpellsKnown { get; set; }                    // list of spells known.

        public static Dictionary<string, string> SaveBonusAssociation = new Dictionary<string, string>()
        {
            {"Fort","Stamina" },
            {"Reflex", "Dexterity" },
            {"Will", "Wisdom" }
        };

        public void CalculateBonusesAndSaves()
        {
            foreach (var AttKVP in Attributes)
            {
                int Bonus = (AttKVP.Value - 10) / 2;
                AttributeBonuses.Add(AttKVP.Key, Bonus);
            }
            foreach (var SaveKVP in SaveBonusAssociation)
            {
                EffectiveSavingThrows[SaveKVP.Key] = BaseSavingThrows[SaveKVP.Key] + AttributeBonuses[SaveKVP.Value];
            }


        }

    }

    
}
