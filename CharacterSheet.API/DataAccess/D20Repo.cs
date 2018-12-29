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
            return (IEnumerable<ClassLibrary.Campaign>) _db.Campaign.Include(c => c.Characters).Include(c => c.Gmjunction);
        }

        public IEnumerable<Character> CharacterList()
        {
            List<Character> ret = (List<Character>) _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).GetEnumerator();
            
            return ret;
        }

        public IEnumerable<Character> CharacterListByCamp(int CampID)
        {
            List<Character> ret = (List<Character>)_db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).Where(c => c.CampaignId == CampID);

            return ret;
        }

        public IEnumerable<Character> CharacterListByUser(int UserID)
        {
            List<Character> ret = (List<Character>)_db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).Where(c => c.GamerId == UserID);

            return ret;
        }

        public Character CharDetails(int CharID)
        {
            return _db.Characters.Include(c => c.Classes).Include(c => c.Feats).Include(c => c.Inventory).Include(c => c.Skills).Include(c => c.SpellJunction).Include(c => c.SpellSlots).First(c => c.CharacterId == CharID);
        }

        public void CreateCampaign(ClassLibrary.Campaign campaign)
        {
            campaign.CampID = 0;
            _db.Campaign.Add(campaign);
            _db.SaveChangesAsync();
        }

        public async void CreateCharacter(Character character)
        {
            character.CharID = 0;
            _db.Characters.Add(character);
            await _db.SaveChangesAsync();
        }

        public async void CreateUser(User user)
        {
            //user.UserID = await _db.Gamer.MaxAsync(g => g.GamerId) + 1;
            user.UserID = 0;
            _db.Gamer.Add(user);
            await _db.SaveChangesAsync();
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

        public User UserDetails(int UserID)
        {
            return _db.Gamer.Find(UserID);
        }

        public IEnumerable<User> UserList()
        {
            return (IEnumerable<User>) _db.Gamer.AsEnumerable();
        }
    }
}
