// ChangeScenes.cs
// By: Harry Jennings-Ramirez
// Last Edited: June 3rd, 2020
// Description: Changes scenes, usually when a button is pushed

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {

    }

    public void OpenGame() {
        SceneManager.LoadScene("LevelTest");
    }

    public void HelpScene() {
        // For later if we decide to add it
        // SceneManager.LoadScene("GameControls");
    }

    public void TitleScene() {
        SceneManager.LoadScene("Menu");
    }

    public void ExitScene() {
        Application.Quit();
    }

    // Update is called once per frame
    void Update() {

    }
}
