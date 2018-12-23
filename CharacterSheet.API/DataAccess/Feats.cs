using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Feats
    {
        public string FeatName { get; set; }
        public int CharacterId { get; set; }
        public int Copies { get; set; }

        public virtual Characters Character { get; set; }
    }
}
