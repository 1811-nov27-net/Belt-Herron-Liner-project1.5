using System;
using System.Collections.Generic;
using ClassLibrary;

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


        public static implicit operator Campaign(ClassLibrary.Campaign campaign)
        {
            throw new NotImplementedException();

        }

        public static implicit operator ClassLibrary.Campaign(Campaign campaign)
        {
            throw new NotImplementedException();

        }
    }
}
