using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

public class GameController : Singleton<GameController> {

    [SerializeField] bool startGameImmediately;
    [SerializeField] string mainLevelSceneName = "main";
    [SerializeField] string mainMenuSceneName = "main menu";

    void Awake() {

        if (!startGameImmediately) {
            ShowMainMenu();
        }
        else {
            StartGame();
        }
    }

    public void ShowMainMenu() {
        
        UnloadAllOtherScenes();
        StartCoroutine(ShowMainMenuCoroutine());
    }

    public void StartGame() {

        SceneManager.UnloadSceneAsync(mainMenuSceneName);
        GlobalEvents.OnGameStart.Invoke();
    }

    public void RestartGame() {

        AsyncOperation result = SceneManager.UnloadSceneAsync(mainLevelSceneName);
        result.completed += (sender) => StartCoroutine(RestartGameCoroutine());
    }

    private void UnloadAllOtherScenes() {

        int ownBuildIndex = gameObject.scene.buildIndex;
        for (int i = SceneManager.sceneCount - 1; i >= 0; --i) {

            if (i >= SceneManager.sceneCount) continue;

            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex == ownBuildIndex) continue;

            SceneManager.UnloadSceneAsync(scene.buildIndex);
        }
    }

    private IEnumerator ShowMainMenuCoroutine() {

        SceneManager.LoadScene(mainLevelSceneName, LoadSceneMode.Additive);
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainLevelSceneName));
    }

    private IEnumerator RestartGameCoroutine() {

        SceneManager.LoadScene(mainLevelSceneName, LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainLevelSceneName));
        GlobalEvents.OnGameStart.Invoke();
        GlobalEvents.OnGameRestart.Invoke();
    }
}
