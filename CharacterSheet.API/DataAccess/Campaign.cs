using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Campaign
    {
        public Campaign()
        {
            Characters = new HashSet<Characters>();
            Gmjunction = new HashSet<Gmjunction>();
        }

        public int CampaignId { get; set; }
        public string CampaignName { get; set; }

        public virtual ICollection<Characters> Characters { get; set; }
        public virtual ICollection<Gmjunction> Gmjunction { get; set; }
    }
}
