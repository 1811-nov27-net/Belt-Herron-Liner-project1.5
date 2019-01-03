using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class D20Repo : IRepo
    {

        private readonly D20CharacterDatabaseContext _db;

        /// <summary>
        /// Initializes a new Character repository given a suitable Entity Framework DbContext.
        /// </summary>
        /// <param name="db">The DbContext</param>
        /// 
        public D20Repo(D20CharacterDatabaseContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void AddGM(int CampID, int UserID)
        {
            _db.Gmjunction.Add(new Gmjunction()
            {
                CampaignId = CampID,
                Gmid = UserID
            });
            _db.SaveChangesAsync();
        }


        public void RemGM(int CampID, int UserID)
        {
            _db.Gmjunction.Remove(_db.Gmjunction.Where(gmj => gmj.CampaignId == CampID && gmj.Gmid == UserID).FirstOrDefault());
            _db.SaveChangesAsync();
        }

        public async Task<ClassLibrary.Campaign> CampDetails(int CampID)
        {
            ClassLibrary.Campaign ret = await _db.Campaign.FindAsync(CampID);
            return ret;
        }

        public async Task<IEnumerable<ClassLibrary.Campaign>> CampList()
        {
            List<ClassLibrary.Campaign> ret = new List<ClassLibrary.Campaign>();
            var temp = await _db.Campaign.Include(c => c.Characters).Include(c => c.Gmjunction).ToListAsync();
            foreach (var item in temp)
            {
                ret.Add(item);
            }
            return ret;
        }

        public async Task<IEnumerable<ClassLibrary.Campaign>> CampList(string GMUsername)
        {
            List<ClassLibrary.Campaign> ret = new List<ClassLibrary.Campaign>();
            var temp = await _db.Campaign.Include(c => c.Characters).Include(c => c.Gmjunction).Where(c => c.Gmjunction.Any(gmj => gmj.Gm.UserName == GMUsername)).ToListAsync();
            if (temp.Count == 0)
                return null;
            foreach (var item in temp)
            {
                ClassLibrary.Campaign camp = item;
                ret.Add(camp);
            }

            return ret;


        }

        public async Task<IEnumerable<Character>> CharacterList()
        {
            List<Character> ret = new List<Character>();
            var temp = await _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).ToListAsync();
            foreach (var item in temp)
            {
                ret.Add(item);
            }


            return ret;
        }

        public async Task<IEnumerable<Character>> CharacterListByCamp(int CampID)
        {
            List<Character> ret = new List<Character>();
            var temp = await _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).Where(c => c.CampaignId == CampID).ToListAsync();
            foreach (var item in temp)
            {
                ret.Add(item);
            }


            return ret;
        }

        public async Task<IEnumerable<Character>> CharacterListByUser(int UserID)
        {
            List<Character> ret = new List<Character>();
            var temp = await _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).Where(c => c.GamerId == UserID).ToListAsync();

            foreach (var item in temp)
            {
                ret.Add(item);
            }

            return ret;
        }

        public async Task<Character> CharDetails(int CharID)
        {
            Character ret = await _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).FirstAsync(c => c.CharacterId == CharID);
            return ret;
        }

        public async Task<Character> CharDetails(string charName)
        {
            Character ret = await _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).FirstAsync(c => c.CharacterName == charName);
            return ret;
        }

        public async Task<int> CreateCampaign(ClassLibrary.Campaign campaign)
        {
            campaign.CampID = 0;
            _db.Campaign.Add(campaign);
            _db.SaveChanges();
            int newId = (await _db.Campaign.FirstAsync(c => c.CampaignName == campaign.Name)).CampaignId;
            return newId;
        }

        public async Task<int> CreateCharacter(Character character)
        {
            character.CharID = 0;
            _db.Characters.Add(character);
            _db.SaveChanges();
            int newId = (await _db.Characters.FirstAsync(c => c.GamerId == character.UserID && c.CharacterName == character.Name && c.Race == character.Race)).CharacterId;
            return newId;
        }

        public async Task<int> CreateUser(User user)
        {
            //int newId = _db.Gamer.Select(c => c.GamerId).Max() + 1;
            user.UserID = 0;
            _db.Gamer.Add(user);
            _db.SaveChanges();
            int newId = (await _db.Gamer.FirstAsync(g => g.UserName == user.Username)).GamerId;
            return newId;
        }

        public void DeleteCamp(int CampID)
        {
            foreach (var item in (_db.Campaign.First(c => c.CampaignId == CampID)).Characters)
            {
                item.CampaignId = 1;
                _db.Characters.Update(item);
            }
            _db.Campaign.Remove(_db.Campaign.First(c => c.CampaignId == CampID));
            _db.SaveChangesAsync();

        }

        public void DeleteChar(int CharID)
        {
            _db.Characters.Remove(_db.Characters.First(c => c.CharacterId == CharID));
            _db.SaveChangesAsync();
        }

        public void DeleteUser(int UserID)
        {
            foreach (var item in (_db.Gamer.First(g => g.GamerId == UserID)).Characters)
            {
                item.GamerId = 1;
                _db.Characters.Update(item);
            }
            _db.Gamer.Remove(_db.Gamer.First(c => c.GamerId == UserID));
            _db.SaveChanges();
        }

        public void JoinCamp(int CampID, int CharID)
        {
            _db.Characters.First(c => c.CharacterId == CharID).CampaignId = CampID;
            _db.SaveChanges();
        }

        public void RemoveCharFromCamp(int CampID, int CharID)
        {
            _db.Characters.First(c => c.CharacterId == CharID).CampaignId = 1; // 1 = no campagin
            _db.SaveChanges();
        }

        public async void UpdateCamp(ClassLibrary.Campaign campaign)
        {
            DataAccess.Campaign camp = campaign;
            var trackedCampaign = await _db.Campaign.FindAsync(campaign.CampID);
            trackedCampaign.CampaignName = camp.CampaignName;
            trackedCampaign.Characters = camp.Characters;
            trackedCampaign.Gmjunction = camp.Gmjunction;
            _db.Campaign.Update(trackedCampaign);
            _db.SaveChanges();
        }

        public async void UpdateCharacter(Character character)
        {
            Characters tempChar = character;
            var trackedCharacter = await _db.Characters.FindAsync(tempChar.CharacterId);
            trackedCharacter.Ac = tempChar.Ac;
            trackedCharacter.Alignment = tempChar.Alignment;
            trackedCharacter.Bab = tempChar.Bab;
            trackedCharacter.BaseFortSave = tempChar.BaseFortSave;
            trackedCharacter.BaseReflexSave = tempChar.BaseReflexSave;
            trackedCharacter.BaseWillSave = tempChar.BaseWillSave;
            trackedCharacter.Campaign = tempChar.Campaign;
            trackedCharacter.CampaignId = tempChar.CampaignId;
            trackedCharacter.CharacterName = tempChar.CharacterName;
            trackedCharacter.Charisma = tempChar.Charisma;
            trackedCharacter.Classes = tempChar.Classes;
            trackedCharacter.Dexterity = tempChar.Dexterity;
            trackedCharacter.Feats = tempChar.Feats;
            trackedCharacter.Ffac = tempChar.Ffac;
            trackedCharacter.Gamer = tempChar.Gamer;
            trackedCharacter.GamerId = tempChar.GamerId;
            trackedCharacter.Intelligence = tempChar.Intelligence;
            trackedCharacter.Inventory = tempChar.Inventory;
            trackedCharacter.MaxHP = tempChar.MaxHP;
            trackedCharacter.Race = tempChar.Race;
            trackedCharacter.Sex = tempChar.Sex;
            trackedCharacter.Skills = tempChar.Skills;
            trackedCharacter.SpellJunction = tempChar.SpellJunction;
            trackedCharacter.SpellSlots = tempChar.SpellSlots;
            trackedCharacter.Stamina = tempChar.Stamina;
            trackedCharacter.Strength = tempChar.Strength;
            trackedCharacter.TouchAc = tempChar.TouchAc;
            trackedCharacter.Wisdom = tempChar.Wisdom;
            _db.Characters.Update(trackedCharacter);
            _db.SaveChanges();
        }

        public async void UpdateUser(User user)
        {
            DataAccess.Gamer gamer = user;
            var trackedUser = await _db.Gamer.FindAsync(gamer.GamerId);
            trackedUser.UserName = gamer.UserName;
            trackedUser.Characters = gamer.Characters;
            trackedUser.Gmjunction = gamer.Gmjunction;
            _db.Gamer.Update(trackedUser);
            _db.SaveChanges();
        }

        public async Task<User> UserDetails(int UserID)
        {
            User ret = await _db.Gamer.FindAsync(UserID);
            return ret;
        }

        public async Task<User> UserDetails(string username)
        {
            User ret = await _db.Gamer.FirstOrDefaultAsync(g => g.UserName == username);
            return ret;
        }

        public async Task<IEnumerable<User>> UserList()
        {
            var temp = await _db.Gamer.ToListAsync();
            List<User> ret = new List<User>();
            foreach (var item in temp)
            {
                ret.Add(item);
            }

            return ret;
        }

        public async Task<IEnumerable<User>> GetGmByCampaign(int CampID)
        {
            var temp = await _db.Gamer.Include(g => g.Gmjunction).Where(g => g.Gmjunction.Any(gm => gm.CampaignId == CampID)).ToListAsync();
            List<User> ret = new List<User>();
            foreach (var item in temp)
            {
                ret.Add(item);
            }

            return ret;
        }
    }
}
