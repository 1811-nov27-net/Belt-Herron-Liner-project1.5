using ClassLibrary;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Characters
    {
        public Characters()
        {
            Classes = new HashSet<Classes>();
            Feats = new HashSet<Feats>();
            Inventory = new HashSet<Inventory>();
            Skills = new HashSet<Skills>();
            SpellJunction = new HashSet<SpellJunction>();
            SpellSlots = new HashSet<SpellSlots>();
        }

        public int CharacterId { get; set; }
        public int GamerId { get; set; }
        public int? CampaignId { get; set; }
        public string Race { get; set; }
        public string Sex { get; set; }
        public string Alignment { get; set; }
        public int MaxHP { get; set; }  //  TODO add to database, make sure Context recognizes this trait
        public int Bab { get; set; }
        public int Ac { get; set; }
        public int TouchAc { get; set; }
        public int Ffac { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Stamina { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int BaseFortSave { get; set; }
        public int BaseReflexSave { get; set; }
        public int BaseWillSave { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Gamer Gamer { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
        public virtual ICollection<Feats> Feats { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Skills> Skills { get; set; }
        public virtual ICollection<SpellJunction> SpellJunction { get; set; }
        public virtual ICollection<SpellSlots> SpellSlots { get; set; }

        public static implicit operator Characters(Character character)  // implicit conversion, takes in library, returns data access
        {
            Characters ret = new Characters();
            ret.CharacterId = character.CharID;
            ret.GamerId = character.UserID;
            ret.CampaignId = character.CampID;



            return ret;

        }

        public static implicit operator Character(Characters character)  // implicit conversion, takes in data access, returns library
        {
            throw new NotImplementedException();

        }
    }
}
