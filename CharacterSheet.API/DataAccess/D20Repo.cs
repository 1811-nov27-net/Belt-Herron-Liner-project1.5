﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    class D20Repo : IRepo
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
        }

        public Campaign CampDetails(int CampID)
        {
            return _db.Campaign.Find(CampID);
        }

        public IEnumerable<Campaign> CampList()
        {
            return _db.Campaign.Include(c => c.Characters).Include(c => c.Gmjunction);
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

        public void CreateCampaign(Campaign campaign)
        {
            _db.Campaign.Add(campaign);
            _db.SaveChanges();
        }

        public void CreateCharacter(Character character)
        {
            _db.Characters.Add(character);
            _db.SaveChanges();
        }

        public void CreateUser(User user)
        {
            _db.Gamer.Add(user);
            _db.SaveChanges();
        }

        public void DeleteCamp(int CampID)
        {
            _db.Campaign.Remove(_db.Campaign.First(c => c.CampaignId == CampID));
            _db.SaveChanges();

        }

        public void DeleteChar(int CharID)
        {
            _db.Characters.Remove(_db.Characters.First(c => c.CharacterId == CharID));
            _db.SaveChanges();
        }

        public void DeleteUser(int UserID)
        {
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
            throw new NotImplementedException();
        }

        public void UpdateCamp(Campaign campaign)
        {
            throw new NotImplementedException();
        }

        public void UpdateCharacter(Character character)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public User UserDetails(int UserID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> UserList()
        {
            throw new NotImplementedException();
        }
    }
}
