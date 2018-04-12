using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;
using TMPro;

#pragma warning disable 0649

public class GameOverScreen : TransitionableScreen {

    [SerializeField] float transitionDuration = 1f;

    [SerializeField] TMP_Text scoreTextMesh;
    [SerializeField] string scoreFormatString = "Score: {0}";

    protected override void Start() {
        
        base.Start();

        Player.instance.OnDeath += (sender) => this.TransitionTo();

        Assert.IsNotNull(scoreTextMesh);
    }

    void Update() {

        if (isCurrentlySelected && Input.GetKeyDown(KeyCode.R)) {
            GameController.instance.RestartGame();
        }
    }

    protected override void OnTransitionIn() {

        canvasGroup.DOFade(1f, transitionDuration).SetEase(Ease.InOutSine);
        scoreTextMesh.text = string.Format(scoreFormatString, Player.instance.score.ToString());
    }

    protected override void OnTransitionOut() {

        canvasGroup.DOFade(0f, transitionDuration).SetEase(Ease.InOutSine);
    }
}
