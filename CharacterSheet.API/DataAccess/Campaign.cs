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
            Campaign ret = new Campaign
            {
                CampaignId = campaign.CampID,
                CampaignName = campaign.Name
            };

            if (campaign.Characters.Count > 0)
            {
                foreach (var kar in campaign.Characters)
                {
                    ret.Characters.Add(kar);
                }
            }

            if (campaign.GMs.Count > 0)
            {
                foreach (var GM in campaign.GMs)
                {
                    ret.Gmjunction.Add(new Gmjunction()
                    {
                        Gmid = GM.UserID,
                        CampaignId = campaign.CampID,
                        Campaign = ret,
                        Gm = GM
                    });
                }
            }
            return ret;

        }

        public static implicit operator ClassLibrary.Campaign(Campaign campaign)
        {
            ClassLibrary.Campaign ret = new ClassLibrary.Campaign
            {
                CampID = campaign.CampaignId,
                Name = campaign.CampaignName
            };


            if (campaign.Characters.Count > 0)
            {
                foreach (var kar in campaign.Characters)
                {
                    ret.Characters.Add(kar);
                }
            }

            if (campaign.Gmjunction.Count > 0)
            {
                //foreach (var GM in campaign.Gmjunction)
                //{
                //    ret.GMs.Add(GM.Gm);
                //}
            }
            return ret;

        }
    }
}
