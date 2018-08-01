using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

public class MainMenu : MonoBehaviour {

    void Start() {

        UnlockCursor();
    }

    public void StartGame() {

        LockCursor();

        GameController.instance.StartGame();
    }

    public void StartTutorial() {

        LockCursor();

        GameController.instance.StartTutorial();
    }

    private void UnlockCursor() {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
