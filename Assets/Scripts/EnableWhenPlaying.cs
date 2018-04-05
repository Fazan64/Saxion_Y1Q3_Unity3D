using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWhenPlaying : MonoBehaviour {

    void Awake() {

        GlobalEvents.OnGameStarted.AddListener(EnableChildren);
    }

    void EnableChildren() {

        for (int i = transform.childCount - 1; i >= 0; --i) {

            if (i >= transform.childCount) continue;

            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
