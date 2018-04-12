using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#pragma warning disable 0649

public class PopupScreen : OverlayScreen {

    [SerializeField] TMP_Text mainTextMesh;

    [SerializeField] Button yesButton;
    [SerializeField] TMP_Text yesTextMesh;

    [SerializeField] Button noButton;
    [SerializeField] TMP_Text noTextMesh;
    
    public void SetText(string text) {

        mainTextMesh.text = text;
    }

    public void SetYesCallback(Action yesCallback) {

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(new UnityAction(yesCallback));
    }

    public void SetNoCallback(Action noCallback) {
        
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(new UnityAction(noCallback));
    }

    public void Set(string text, Action yesCallback, Action noCallback) {

        SetText(text);
        SetYesCallback(yesCallback);
        SetNoCallback(noCallback);
    }
}
