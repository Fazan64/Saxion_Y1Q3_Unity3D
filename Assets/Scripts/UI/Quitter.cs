﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitter : MonoBehaviour {

    public void QuitToMenu() {

        GameController.instance.ShowMainMenu();
    }

    public void QuitToDesktop() {
        
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif

    }
}
