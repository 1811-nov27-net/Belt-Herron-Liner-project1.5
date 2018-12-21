using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;

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

        public Campaign CampDetails(int CampID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Campaign> CampList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Character> CharacterList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Character> CharacterListByCamp(int CampID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Character> CharacterListByUser(int UserID)
        {
            throw new NotImplementedException();
        }

        public Character CharDetails(int CharID)
        {
            throw new NotImplementedException();
        }

        public void CreateCampaign(Campaign campaign)
        {
            throw new NotImplementedException();
        }

        public void CreateCharacter(Character character)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteCamp(int CampID)
        {
            throw new NotImplementedException();
        }

        public void DeleteChar(int CharID)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int UserID)
        {
            throw new NotImplementedException();
        }

        public void JoinCamp(int CampID, int CharID)
        {
            throw new NotImplementedException();
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
