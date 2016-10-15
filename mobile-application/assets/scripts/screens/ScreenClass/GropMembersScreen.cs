using UnityEngine;
using System.Collections;
using Amigosi.Screen;
using System.Collections.Generic;

public class GropMembersScreen : Amigosi.Screen.Screen
{
    private List<GameObject> applicationList;
    public GameObject[] child;
    void Start()
    {
        applicationList = new List<GameObject>();
    }

    public void FinishButton()
    {

    }
}
