using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static implicit operator User(Gamer gamer)
        {
            User user = new User
            {
                UserID = gamer.GamerId,
                Username = gamer.UserName
            };

            foreach (var Character in gamer.Characters)
            {
                user.Characters.Add(Character);
            }

            foreach (var Campagin in gamer.Gmjunction.Select(j => j.Campaign))
            {
                user.MyCampaigns.Add(Campagin);
            }

            return user;
        }

        public static implicit operator Gamer(User user)
        {
            Gamer gamer = new Gamer
            {
                UserName = user.Username,
                GamerId = user.UserID
            };

            foreach (var Character in user.Characters)
            {
                gamer.Characters.Add(Character);
            }

            foreach (var Camp in user.MyCampaigns)
            {
                gamer.Gmjunction.Add(new Gmjunction
                {
                    CampaignId = Camp.CampID,
                    Gmid = user.UserID,
                    Gm = gamer,
                    Campaign = Camp

                });
            }

            return gamer;

        }
    }
}
