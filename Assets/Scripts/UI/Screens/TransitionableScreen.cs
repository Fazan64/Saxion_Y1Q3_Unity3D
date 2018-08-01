using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

#pragma warning disable 0649

[RequireComponent(typeof(CanvasGroup))]
public class TransitionableScreen : MonoBehaviour {

    protected static Stack<TransitionableScreen> previousScreens = new Stack<TransitionableScreen>();
    protected static TransitionableScreen currentlySelected;

    [SerializeField] bool startSelected;
    [SerializeField] bool deactivateOnTransitionOut;

    [SerializeField] UnityEvent onTransitionIn  = new UnityEvent();
    [SerializeField] UnityEvent onTransitionOut = new UnityEvent();

    protected CanvasGroup canvasGroup;

    public bool wasSelectedLastFrame { get; private set; }

    public bool isCurrentlySelected {
        get {
            return this == currentlySelected; 
        }
    }

    protected virtual void Start() {

        canvasGroup = GetComponent<CanvasGroup>();

        if (startSelected) this.TransitionTo();
    }

    protected virtual void LateUpdate() {

        wasSelectedLastFrame = isCurrentlySelected;
    }

    public void TransitionTo() {

        if (this.isCurrentlySelected) return;

        if (currentlySelected != null) {
            
            Deactivate(currentlySelected);
            previousScreens.Push(currentlySelected);
        }

        currentlySelected = this;
        Activate(this);
    }

    public static void TransitionToPrevious() {

        if (previousScreens.Count == 0) return;

        if (currentlySelected != null) {
            Deactivate(currentlySelected);
        }

        currentlySelected = previousScreens.Pop();
        Activate(currentlySelected);
    }

    protected virtual void OnTransitionIn()  {}
    protected virtual void OnTransitionOut() {}

    private static void Activate(TransitionableScreen screen) {

        var canvasGroup = screen.canvasGroup ?? screen.GetComponent<CanvasGroup>();
        canvasGroup.interactable   = true;
        canvasGroup.blocksRaycasts = true;

        screen.gameObject.SetActive(true);
        screen.OnTransitionIn();
        screen.onTransitionIn.Invoke();
    }

    private static void Deactivate(TransitionableScreen screen) {

        var canvasGroup = screen.canvasGroup ?? screen.GetComponent<CanvasGroup>();
        canvasGroup.interactable   = false;
        canvasGroup.blocksRaycasts = false;

        if (screen.deactivateOnTransitionOut) {
            screen.gameObject.SetActive(false);
        }
        screen.OnTransitionOut();
        screen.onTransitionOut.Invoke();
    }
}
