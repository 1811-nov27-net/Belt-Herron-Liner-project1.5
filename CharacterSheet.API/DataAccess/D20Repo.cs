﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _db.Campaign.Add(campaign);
            _db.SaveChangesAsync();
        }

        public void CreateCharacter(Character character)
        {
            _db.Characters.Add(character);
            _db.SaveChangesAsync();
        }

        public void CreateUser(User user)
        {
            _db.Gamer.Add(user);
            _db.SaveChangesAsync();
        }

        public void DeleteCamp(int CampID)
        {
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
            _db.Characters.First(c => c.CharacterId == CharID).CampaignId = 0; // 0 = no campagin
            _db.SaveChangesAsync();
        }

        public void UpdateCamp(ClassLibrary.Campaign campaign)
        {
            _db.Campaign.Update(campaign);
            _db.SaveChangesAsync();
        }

        public void UpdateCharacter(Character character)
        {
            _db.Characters.Update(character);
            _db.SaveChangesAsync();
        }

        public void UpdateUser(User user)
        {
            _db.Gamer.Update(user);
            _db.SaveChangesAsync();
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
