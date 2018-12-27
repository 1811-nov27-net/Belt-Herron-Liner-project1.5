﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.MVC.Models
{
    public class Spell
    {
        public int SpellId { get; set; }
        public string Name { get; set; }
        public int SpellLevel { get; set; }
        public string Class { get; set; }      
    }
}
