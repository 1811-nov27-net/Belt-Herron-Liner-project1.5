using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Skills
    {
        public string SkillName { get; set; }
        public int CharacterId { get; set; }
        public int Levels { get; set; }

        public virtual Characters Character { get; set; }
    }
}
