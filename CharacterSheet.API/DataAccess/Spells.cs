using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Spells
    {
        public Spells()
        {
            SpellJunction = new HashSet<SpellJunction>();
        }

        public int SpellId { get; set; }
        public string SpellName { get; set; }
        public string Class { get; set; }
        public int SpellLevel { get; set; }

        public virtual ICollection<SpellJunction> SpellJunction { get; set; }
    }
}
