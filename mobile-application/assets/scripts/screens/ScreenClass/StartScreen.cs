using UnityEngine;
using System.Collections;
using Amigosi.Screen;
using Amigosi.Data;
using UnityEngine.UI;

public class StartScreen : Amigosi.Screen.Screen
{
    public Text btnRegister;

    void OnEnable()
    {
        Debug.Log("User" + user.getViewID());
        if (user.getViewID() == "")       
            btnRegister.text = "Register";        
        else
            btnRegister.text = "Continue";
    }

    public void registerButton()
    {
        if (btnRegister.text == "Register")
        {
            screenManager.StartScreen("RegistrationScreen");
        }
        if (btnRegister.text == "Continue")
        {
            screenManager.StartScreen("MainScreen");
        }
    }

    public void exitButton()
    {
        Application.Quit();
    }


}