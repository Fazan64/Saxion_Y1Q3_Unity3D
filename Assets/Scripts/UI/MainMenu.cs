using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    void Awake() {

        GlobalEvents.OnGameStarted.AddListener(() => gameObject.SetActive(false));

        Cursor.visible = true;
    }

    public void StartGame() {

        GlobalEvents.OnGameStarted.Invoke();
    }
}
