using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

public class GameController : Singleton<GameController> {

    [SerializeField] bool loadMainMenuOnStart;
    [SerializeField] string mainLevelSceneName = "main level";
    [SerializeField] string mainMenuSceneName  = "main menu";
    [SerializeField] string tutorialSceneName  = "tutorial";

    void Awake() {

        DontDestroyOnLoad(this);
    }

    void Start() {

        if (loadMainMenuOnStart) {
            ShowMainMenu();
        }
    }

    public void ShowMainMenu() {

        DG.Tweening.DOTween.Clear(destroy: true);
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void StartGame() {

        DG.Tweening.DOTween.Clear(destroy: true);
        SceneManager.LoadScene(mainLevelSceneName);
    }

    public void StartTutorial() {

        DG.Tweening.DOTween.Clear(destroy: true);
        SceneManager.LoadScene(tutorialSceneName);
    }

    public void RestartGame() {

        DG.Tweening.DOTween.Clear(destroy: true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
