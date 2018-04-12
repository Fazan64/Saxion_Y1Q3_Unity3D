using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#pragma warning disable 0649

[RequireComponent(typeof(RectTransform))]
public class OverlayScreen : TransitionableScreen {

    [SerializeField] float transitionDurationIn  = 0.5f;
    [SerializeField] float transitionDurationOut = 0.5f;

    RectTransform rectTransform;

    void Awake() {

        rectTransform = GetComponent<RectTransform>();
        SetOffsets(new Vector2(0f, rectTransform.rect.height));
    }

    protected override void OnTransitionIn() {

        DOTween.To(
            () => rectTransform.offsetMin,
            SetOffsets,
            Vector2.zero,
            transitionDurationIn
        ).SetEase(Ease.OutExpo);
    }

    protected override void OnTransitionOut() {

        DOTween.To(
            () => rectTransform.offsetMin,
            SetOffsets,
            new Vector2(0f, rectTransform.rect.height),
            transitionDurationOut
        ).SetEase(Ease.InQuad);
    }

    private void SetOffsets(Vector2 newOffsets) {

        rectTransform.offsetMin = newOffsets;
        rectTransform.offsetMax = newOffsets;
    }
}
