using System;
using System.Collections.Generic;
using System.Text;
using Lib = ClassLibrary;

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
            throw new NotImplementedException();
        }

        public Lib.Campaign CampDetails(int CampID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.Campaign> CampList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.Character> CharacterList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.Character> CharacterListByCamp(int CampID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.Character> CharacterListByUser(int UserID)
        {
            throw new NotImplementedException();
        }

        public Lib.Character CharDetails(int CharID)
        {
            throw new NotImplementedException();
        }

        public void CreateCampaign(Lib.Campaign campaign)
        {
            throw new NotImplementedException();
        }

        public void CreateCharacter(Lib.Character character)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(Lib.User user)
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

        public void UpdateCamp(Lib.Campaign campaign)
        {
            throw new NotImplementedException();
        }

        public void UpdateCharacter(Lib.Character character)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Lib.User user)
        {
            throw new NotImplementedException();
        }

        public Lib.User UserDetails(int UserID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lib.User> UserList()
        {
            throw new NotImplementedException();
        }
    }
}
