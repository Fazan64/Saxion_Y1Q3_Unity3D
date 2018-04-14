using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

public class GameController : Singleton<GameController> {

    [SerializeField] bool startGameImmediately;
    [SerializeField] string mainLevelSceneName = "main level";
    [SerializeField] string mainMenuSceneName  = "main menu";

    public bool isPaused { get; private set; }

    private bool _canPause = true;
    public bool canPause {
        get {
            return _canPause;
        }
        set {
            _canPause = value; 
        }
    }

    private float timeScaleBeforePause = 1f;

    /*void Awake() {

        DontDestroyOnLoad(this);
    }*/

    void Start() {

        if (!startGameImmediately) {
            ShowMainMenu();
        } else {
            StartGame();
        }
    }

    void Update() {
        
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (isPaused) {
                Unpause();
            } else {
                Pause();
            }
        }
    }

    public void ShowMainMenu() {
        
        UnloadAllOtherScenes();
        StartCoroutine(ShowMainMenuCoroutine());
    }

    public void StartGame() {

        UnloadAllOtherScenes();

        AsyncOperation loading = SceneManager.LoadSceneAsync(mainLevelSceneName, LoadSceneMode.Additive);
        loading.completed += (sender) => {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainLevelSceneName));
        };
    }

    public void RestartGame() {

        DG.Tweening.DOTween.Clear(destroy: true);

        UnloadAllOtherScenes();
        StartCoroutine(RestartGameCoroutine());

        //AsyncOperation result = SceneManager.UnloadSceneAsync(mainLevelSceneName);
        //result.completed += (sender) => StartCoroutine(RestartGameCoroutine());
    }

    public void Pause() {

        if (!canPause) return;
        if (isPaused) return;

        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;

        isPaused = true;
        GlobalEvents.OnGamePause.Invoke();
    }

    public void Unpause() {

        if (!isPaused) return;

        Time.timeScale = timeScaleBeforePause;

        isPaused = false;
        GlobalEvents.OnGameUnpause.Invoke();
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

        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainMenuSceneName));
    }

    private IEnumerator RestartGameCoroutine() {

        SceneManager.LoadScene(mainLevelSceneName, LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainLevelSceneName));
        GlobalEvents.OnGameRestart.Invoke();
    }
}
