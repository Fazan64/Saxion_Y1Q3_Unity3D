using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using DG.Tweening;
using TMPro;

public class PauseScreen : TransitionableScreen {

    [SerializeField] float transitionDuration = 1f;

    protected override void Start() {

        base.Start();

        GlobalEvents.OnGamePause.AddListener(TransitionTo);
        GlobalEvents.OnGameUnpause.AddListener(TransitionToPrevious);
    }

    void OnDestroy() {

        GlobalEvents.OnGamePause.RemoveListener(TransitionTo);
        GlobalEvents.OnGameUnpause.RemoveListener(TransitionToPrevious);
    }

    protected override void OnTransitionIn() {

        canvasGroup.DOKill();
        canvasGroup
            .DOFade(1f, transitionDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(isIndependentUpdate: true);
    }

    protected override void OnTransitionOut() {

        canvasGroup.DOKill();
        canvasGroup
            .DOFade(0f, transitionDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(isIndependentUpdate: true);
    }
}
