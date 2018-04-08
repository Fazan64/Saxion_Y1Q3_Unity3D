using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

public class MainMenu : MonoBehaviour {

    void Start() {

        Cursor.visible = true;
    }

    public void StartGame() {

        GameController.instance.StartGame();
    }
}
