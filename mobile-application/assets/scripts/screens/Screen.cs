using UnityEngine;
using System.Collections;
using System;
using Amigosi.Data;
namespace Amigosi.Screen
{
    //Osnovna klasa za aplikacione prikaze
    public class Screen:MonoBehaviour {
        //Osnovni podatci za vezu sa podatcimoa o aplikaciji 
        public AppData appData;
        public User user;
        public ScreenManager screenManager;
    }
}
