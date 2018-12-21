using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.MVC.Models
{
    public class Spell
    {
        public int SpellId { get; set; }
        public string Name { get; set; }
        public Dictionary<string, int> ClassAndLevel { get; set; }      // Class name, spell level for that class.
    }
}
