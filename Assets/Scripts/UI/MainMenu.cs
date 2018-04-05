using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class MainMenu : MonoBehaviour {

    [SerializeField] bool startGameImmediately;

    void Awake() {

        GlobalEvents.OnGameStarted.AddListener(() => gameObject.SetActive(false));
    }

    void Start() {

        Cursor.visible = true;
        if (startGameImmediately) StartGame();
    }

    public void StartGame() {

        GlobalEvents.OnGameStarted.Invoke();
    }
}
