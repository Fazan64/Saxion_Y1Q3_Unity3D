using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

#pragma warning disable 0649

public class ScoreView : MonoBehaviour {

    [SerializeField] TMP_Text textMesh;
    [Space]
    [SerializeField] float animationScale = 0.5f;
    [SerializeField] float animationDuration = 0.2f;
    [SerializeField] int animationVibrato = 10;

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

        textMesh.rectTransform.DOKill(complete: true);
        textMesh.rectTransform.DOPunchScale(Vector3.one * animationScale, animationDuration, animationVibrato);
    }
}
