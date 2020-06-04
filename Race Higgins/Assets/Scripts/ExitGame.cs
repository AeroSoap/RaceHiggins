/*
 ExitGame.cs
 By: Liam Binford
 Date: 6/4/20
 Description: Closes the game when esc is pressed
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the player presses escape, quit the game
        if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
