using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class SpellJunction
    {
        public int CharacterId { get; set; }
        public int SpellId { get; set; }

        public virtual Characters Character { get; set; }
        public virtual Spells Spell { get; set; }
    }
}
