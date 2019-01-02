using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface IRepo
    {
        void AddGM(int CampID, int UserID);
        void RemGM(int CampID, int UserID);
        Task<IEnumerable<User>> GetGmByCampaign(int CampID);

        //for creating new instances in the database
        Task<int> CreateCharacter(Character character);                             //create a new character
        Task<int> CreateCampaign(Campaign campaign);                              //create a new campaign
        Task<int> CreateUser(User user);                                  //create a new user

        //for reading individual entries
        Task<Character> CharDetails(int CharID);                            //view character details
        Task<Character> CharDetails(string charName);
        Task<Campaign> CampDetails(int CampID);                             //view campaign details
        Task<User> UserDetails(int UserID);                                 //view user details
        Task<User> UserDetails(string username);

        //for updating entries
        void UpdateCharacter(Character character);                             //update a character
        void UpdateCamp(Campaign campaign);                                  //update a campaign
        void UpdateUser(User user);                                  //update a user

        //for deleting entries
        void DeleteChar(int CharID);                                  //delete a character
        void DeleteCamp(int CampID);                                  //delete a campaign
        void DeleteUser(int UserID);                                  //delete a user

        //for retrieving basic lists
        Task<IEnumerable<Character>> CharacterList();             //grab the character list
        Task<IEnumerable<Campaign>> CampList();                   //grab the campaign list
        Task<IEnumerable<Campaign>> CampList(string GMUsername);
        Task<IEnumerable<User>> UserList();                       //grab the user list

        //for retrieving specified lists
        Task<IEnumerable<Character>> CharacterListByUser(int UserID);       //list of characters for a certain user
        Task<IEnumerable<Character>> CharacterListByCamp(int CampID);       //list of characters for a certain campaign

        //user functions
        void JoinCamp(int CampID, int CharID);                                    //join a campaign

        //GM functions
        void RemoveCharFromCamp(int CampID, int CharID);                          //removes a character from a campaign


    }
}
