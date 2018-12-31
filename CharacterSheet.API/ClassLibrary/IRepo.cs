﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;

namespace DataAccess
{
    public interface IRepo
    {
        /// <summary>
        /// Interfaces with D20 character database, extracting Library classes from SQL.
        /// All methods that make changes to the database (saving, updating, deleting)
        /// also save those changes. 
        /// </summary>
        /// <param name="character"></param>


        //for creating new instances in the database
        int CreateCharacter(Character character);                             //create a new character
        int CreateCampaign(Campaign campaign);                              //create a new campaign
        int CreateUser(User user);                                  //create a new user

        //for reading individual entries
        Character CharDetails(int CharID);                            //view character details
        Campaign CampDetails(int CampID);                             //view campaign details
        User UserDetails(int UserID);                                 //view user details

        //for updating entries
        void UpdateCharacter(Character character);                             //update a character
        void UpdateCamp(Campaign campaign);                                  //update a campaign
        void UpdateUser(User user);                                  //update a user

        //for deleting entries
        void DeleteChar(int CharID);                                  //delete a character
        void DeleteCamp(int CampID);                                  //delete a campaign
        void DeleteUser(int UserID);                                  //delete a user

        //for retrieving basic lists
        IEnumerable<Character> CharacterList();             //grab the character list
        IEnumerable<Campaign> CampList();                   //grab the campaign list
        IEnumerable<User> UserList();                       //grab the user list

        //for retrieving specified lists
        IEnumerable<Character> CharacterListByUser(int UserID);       //list of characters for a certain user
        IEnumerable<Character> CharacterListByCamp(int CampID);       //list of characters for a certain campaign

        //user functions
        void JoinCamp(int CampID, int CharID);                                    //join a campaign
        

        //GM functions
        void RemoveCharFromCamp(int CampID, int CharID);                          //removes a character from a campaign
        void AddGM(int CampID, int UserID);

    }
}
