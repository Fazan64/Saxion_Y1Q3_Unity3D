﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using DG.Tweening;
using TMPro;

public class PauseScreen : TransitionableScreen {

    [SerializeField] float transitionDuration = 1f;
    [SerializeField] float maxScale = 2f;

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

        transform.DOKill();
        transform.localScale = Vector3.one;
        transform
            .DOScale(maxScale, transitionDuration)
            .From()
            .SetEase(Ease.OutExpo)
            .SetUpdate(isIndependentUpdate: true);

        GetComponentInChildren<Button>().Select();
    }

    protected override void OnTransitionOut() {

        canvasGroup.DOKill();
        canvasGroup
            .DOFade(0f, transitionDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(isIndependentUpdate: true);

        transform.DOKill(complete: true);
        transform
            .DOScale(maxScale, transitionDuration)
            .SetEase(Ease.InExpo)
            .SetUpdate(isIndependentUpdate: true);
    }
}