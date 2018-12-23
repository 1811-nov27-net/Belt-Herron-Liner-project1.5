using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class SpellSlots
    {
        public int CharacterId { get; set; }
        public string ClassName { get; set; }
        public int? Level0Slots { get; set; }
        public int? Level1Slots { get; set; }
        public int? Level2Slots { get; set; }
        public int? Level3Slots { get; set; }
        public int? Level4Slots { get; set; }
        public int? Level5Slots { get; set; }
        public int? Level6Slots { get; set; }
        public int? Level7Slots { get; set; }
        public int? Level8Slots { get; set; }
        public int? Level9Slots { get; set; }

        public virtual Characters Character { get; set; }
    }
}
