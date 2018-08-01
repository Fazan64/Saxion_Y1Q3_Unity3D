using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#pragma warning disable 0649

public class Blinking : MonoBehaviour {

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float blinkDuration = 1f;

    private void Awake() {

        canvasGroup.alpha = 0f;
    }

    void Start() {
        
        canvasGroup
            .DOFade(1f, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
