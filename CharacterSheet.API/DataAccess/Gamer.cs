using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Gamer
    {
        public Gamer()
        {
            Characters = new HashSet<Characters>();
            Gmjunction = new HashSet<Gmjunction>();
        }

        public int GamerId { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Characters> Characters { get; set; }
        public virtual ICollection<Gmjunction> Gmjunction { get; set; }
    }
}
