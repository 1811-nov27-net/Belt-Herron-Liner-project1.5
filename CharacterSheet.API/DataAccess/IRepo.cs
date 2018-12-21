using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;

namespace DataAccess
{
    interface IRepo
    {
        //for creating new instances in the database
        void CreateCharacter(/*args*/);                             //create a new character
        void CreateCampaign(/*args*/);                              //create a new campaign
        void CreateUser(/*args*/);                                  //create a new user

        //for reading individual entries
        Character CharDetails(/*args*/);                            //view character details
        Campaign CampDetails(/*args*/);                             //view campaign details
        User UserDetails(/*args*/);                                 //view user details

        //for updating entries
        void UpdateCharacter(/*args*/);                             //update a character
        void UpdateCamp(/*args*/);                                  //update a campaign
        void UpdateUser(/*args*/);                                  //update a user

        //for deleting entries
        void DeleteChar(/*args*/);                                  //delete a character
        void DeleteCamp(/*args*/);                                  //delete a campaign
        void DeleteUser(/*args*/);                                  //delete a user

        //for retrieving basic lists
        IEnumerable CharacterList<Character>(/*args*/);             //grab the character list
        IEnumerable CampList<Campaign>(/*args*/);                   //grab the campaign list
        IEnumerable UserList<User>(/*args*/);                       //grab the user list

        //for retrieving specified lists
        IEnumerable CharacterListByUser<Character>(/*args*/);       //list of characters for a certain user
        IEnumerable CharacterListByCamp<Character>(/*args*/);       //list of characters for a certain campaign

        //user functions
        void JoinCamp(/*args*/);                                    //join a campaign

        //GM functions
        void RemoveCharFromCamp(/*args*/);                          //removes a character from a campaign


    }
}
