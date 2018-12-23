using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Gmjunction
    {
        public int CampaignId { get; set; }
        public int Gmid { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Gamer Gm { get; set; }
    }
}
