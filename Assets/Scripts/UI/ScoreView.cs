using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#pragma warning disable 0649

public class ScoreView : MonoBehaviour {

    [SerializeField] TMP_Text textMesh;

    Player player;
    int scoreLastUpdate;

    void Start() {

        player = Player.instance;

        UpdateText();
        scoreLastUpdate = player.score;
    }

    void Update() {

        if (player.score != scoreLastUpdate) {
            UpdateText();
        }

        scoreLastUpdate = player.score;
    }

    private void UpdateText() {

        textMesh.text = player.score.ToString();
    }
}
