using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TMP_Text))]
public class ScoreBonusText : MonoBehaviour {

    [SerializeField] TMP_Text textMesh;
    [SerializeField] float animationDuration = 0.5f;
    [SerializeField] float maxScale = 2f;

    void Start() {

        textMesh = textMesh ?? GetComponent<TMP_Text>();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(textMesh.transform.DOScale(maxScale, animationDuration * 0.5f));
        sequence.Append(textMesh.transform.DOScale(0f, animationDuration * 0.5f));

        sequence.SetEase(Ease.OutQuad);
        sequence.OnComplete(() => Destroy(gameObject));
    }

    void Update() {

        Vector3 fromPlayer = transform.position - Player.instance.transform.position;
        transform.rotation = Quaternion.LookRotation(fromPlayer.normalized, Vector3.up);
    }
}
