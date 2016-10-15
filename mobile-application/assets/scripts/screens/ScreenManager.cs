using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

namespace Amigosi.Screen
{
    [RequireComponent(typeof(Animator))]
    //Ova klasa trea da je  na ojektu koji je nezavistan od scena
    public class ScreenManager : MonoBehaviour {
        #region Atributi
        //Stek za funkcionalnost povratka u predhodni ekran
        private Stack<string> history;
        //Lista svih pogleda popunjava se manuelno u startu INICIJALIZUJE SE U EDITORU
        public string[] screens;
        //Animator tranzicija izmedju ekrana
        public Animator screenAnimator;
        #endregion
        #region Inicijalizacija ovde ide dodavanje svih pogleda u listu
        void Awake()
        {
            history = new Stack<string>();
            history.Push(screens[0]);
        }
        #endregion
        #region Metode 
        public void StartScreen(string screen)
        {
            if (screen == history.Peek())
                return;
            if(history.Contains(screen))
            {
                while (history.Pop() != screen || history.Count == 0) ;
            }

            history.Push(screen);
            screenAnimator.Play(screen);
        }
        public void GoBackOneScreen()
        {
            if (history.Count <=1)
                return;
            history.Pop();
            screenAnimator.Play(history.Peek());
        }
        #endregion
    }
}
