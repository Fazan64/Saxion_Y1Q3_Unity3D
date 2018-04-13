using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using DG.Tweening;
using TMPro;

#pragma warning disable 0649

public class GameOverScreen : TransitionableScreen {

    [SerializeField] float transitionDuration = 1f;

    [SerializeField] TMP_Text scoreTextMesh;
    [SerializeField] string scoreFormatString = "Score: {0}";
    [SerializeField] Image background;
    [SerializeField] float finalBackgroundAlpha = 1f;
    [SerializeField] float backgroundWhiteningDuration = 10f;

    protected override void Start() {
        
        base.Start();

        Player.instance.OnDeath += (sender) => this.TransitionTo();

        Assert.IsNotNull(scoreTextMesh);
        Assert.IsNotNull(background);
    }

    void Update() {

        if (isCurrentlySelected && Input.GetKeyDown(KeyCode.R)) {
            GameController.instance.RestartGame();
        }
    }

    protected override void OnTransitionIn() {

        canvasGroup.DOFade(1f, transitionDuration).SetEase(Ease.InOutSine);
        scoreTextMesh.text = string.Format(scoreFormatString, Player.instance.score.ToString());

        Color newColor = background.color;
        newColor.a = finalBackgroundAlpha;
        background.DOColor(newColor, backgroundWhiteningDuration).SetEase(Ease.OutSine);
    }
}
