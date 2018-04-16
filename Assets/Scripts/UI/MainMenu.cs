using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

public class MainMenu : MonoBehaviour {

    void Start() {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame() {

        GameController.instance.StartGame();
    }

    public void StartTutorial() {

        GameController.instance.StartTutorial();
    }
}
