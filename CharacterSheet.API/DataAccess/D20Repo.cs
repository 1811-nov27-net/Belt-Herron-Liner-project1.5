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

        public ClassLibrary.Campaign CampDetails(int CampID)
        {

            return _db.Campaign.Find(CampID);
        }

        public IEnumerable<ClassLibrary.Campaign> CampList()
        {
            List<ClassLibrary.Campaign> ret = new List<ClassLibrary.Campaign>();
            var temp = _db.Campaign.Include(c => c.Characters).Include(c => c.Gmjunction).ToList();
            foreach (var item in temp)
            {
                ret.Add(item);
            }
            return ret;
        }

        public IEnumerable<ClassLibrary.Campaign> CampList(string GMUsername)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClassLibrary.Character> CharacterList()
        {
            List<Character> ret = new List<Character>();
            var temp = _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).ToList();
            foreach (var item in temp)
            {
                ret.Add(item);
            }


            return ret;
        }

        public IEnumerable<ClassLibrary.Character> CharacterListByCamp(int CampID)
        {
            List<Character> ret = new List<Character>();
            var temp = _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).Where(c => c.CampaignId == CampID);
            foreach (var item in temp)
            {
                ret.Add(item);
            }


            return ret;
        }

        public IEnumerable<ClassLibrary.Character> CharacterListByUser(int UserID)
        {
            List<Character> ret = new List<Character>();
            var temp = _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).Where(c => c.GamerId == UserID);

            foreach (var item in temp)
            {
                ret.Add(item);
            }

            return ret;
        }

        public ClassLibrary.Character CharDetails(int CharID)
        {
            return _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).First(c => c.CharacterId == CharID);
        }

        public Character CharDetails(string username)
        {
            throw new NotImplementedException();
        }

        public int CreateCampaign(ClassLibrary.Campaign campaign)
        {
            int newId = _db.Campaign.Select(c => c.CampaignId).Max() + 1;
            campaign.CampID = newId;
            _db.Campaign.Add(campaign);
            _db.SaveChangesAsync();
            return newId;
        }

        public int CreateCharacter(Character character)
        {
            int newId = _db.Characters.Select(c => c.CharacterId).Max() + 1;
            character.CharID = newId;
            _db.Characters.Add(character);
            _db.SaveChangesAsync();
            return newId;
        }

        public int CreateUser(User user)
        {
            //user.UserID = await _db.Gamer.MaxAsync(g => g.GamerId) + 1;
            int newId = _db.Gamer.Select(c => c.GamerId).Max() + 1;
            user.UserID = newId;
            _db.Gamer.Add(user);
            _db.SaveChangesAsync();
            return newId;
        }

        public void DeleteCamp(int CampID)
        {
            foreach (var item in _db.Campaign.First(c => c.CampaignId == CampID).Characters)
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
            foreach (var item in _db.Gamer.First(g => g.GamerId == UserID).Characters)
            {
                item.GamerId = 1;
                _db.Characters.Update(item);
            }
            _db.Gamer.Remove(_db.Gamer.First(c => c.GamerId == UserID));
            _db.SaveChangesAsync();
        }

        public void JoinCamp(int CampID, int CharID)
        {
            _db.Characters.First(c => c.CharacterId == CharID).CampaignId = CampID;
            _db.SaveChangesAsync();
        }

        public void RemoveCharFromCamp(int CampID, int CharID)
        {
            _db.Characters.First(c => c.CharacterId == CharID).CampaignId = 1; // 1 = no campagin
            _db.SaveChangesAsync();
        }

        public async void UpdateCamp(ClassLibrary.Campaign campaign)
        {
            DataAccess.Campaign camp = campaign;
            var trackedCampaign = await _db.Campaign.FindAsync(campaign.CampID);
            trackedCampaign.CampaignName = camp.CampaignName;
            trackedCampaign.Characters = camp.Characters;
            trackedCampaign.Gmjunction = camp.Gmjunction;
            _db.Campaign.Update(trackedCampaign);
            await _db.SaveChangesAsync();
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
            await _db.SaveChangesAsync();
        }

        public async void UpdateUser(User user)
        {
            DataAccess.Gamer gamer = user;
            var trackedUser = await _db.Gamer.FindAsync(gamer.GamerId);
            trackedUser.UserName = gamer.UserName;
            trackedUser.Characters = gamer.Characters;
            trackedUser.Gmjunction = gamer.Gmjunction;
            _db.Gamer.Update(trackedUser);
            await _db.SaveChangesAsync();
        }

        public ClassLibrary.User UserDetails(int UserID)
        {
            return _db.Gamer.Find(UserID);
        }

        public IEnumerable<ClassLibrary.User> UserList()
        {
            var temp = _db.Gamer.AsEnumerable();
            List<User> ret = new List<User>();
            foreach (var item in temp)
            {
                ret.Add(item);
            }

            return ret;
        }
    }
}
