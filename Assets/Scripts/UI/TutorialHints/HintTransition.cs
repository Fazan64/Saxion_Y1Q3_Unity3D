using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class HintTransition : MonoBehaviour {

    [SerializeField] float transitionDuration = 0.25f;
    [SerializeField] float maxScale = 2f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public bool isTransitionedIn { get; private set; }

    [SerializeField] UnityEvent _onTransitionIn = new UnityEvent();
    public UnityEvent onTransitionIn { get { return _onTransitionIn; } }
    [SerializeField] UnityEvent _onTransitionOut = new UnityEvent();
    public UnityEvent onTransitionOut { get { return _onTransitionOut; } }

    protected virtual void Start() {

        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void TransitionIn() {

        canvasGroup.DOKill();
        canvasGroup
            .DOFade(1f, transitionDuration)
            .SetEase(Ease.InOutSine);

        transform.DOKill();
        transform.localScale = Vector3.one;
        transform
            .DOScale(maxScale, transitionDuration)
            .From()
            .SetEase(Ease.OutExpo);

        isTransitionedIn = true;
        onTransitionIn.Invoke();
    }

    public void TransitionOut() {

        canvasGroup.DOKill();
        canvasGroup
            .DOFade(0f, transitionDuration)
            .SetEase(Ease.InOutSine);

        transform.DOKill(complete: true);
        transform
            .DOScale(maxScale, transitionDuration)
            .SetEase(Ease.InExpo);

        isTransitionedIn = false;
        onTransitionOut.Invoke();
    }
}
