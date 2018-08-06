using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

public class MainMenu : MonoBehaviour {

    void Start() {

        CursorHelper.Unlock();
    }

    public void StartGame() {

        CursorHelper.Lock();

        GameController.instance.StartGame();
    }

    public void StartTutorial() {

        CursorHelper.Lock();

        GameController.instance.StartTutorial();
    }
}
