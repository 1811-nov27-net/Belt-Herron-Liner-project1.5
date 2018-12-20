using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    interface IRepo
    {
        void CreateCharacter(/*args*/);                     //create a new character
        IEnumerable CharacterList<Character>(/*args*/);     //grab a character list

    }
}
