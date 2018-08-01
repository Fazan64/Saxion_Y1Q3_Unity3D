using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#pragma warning disable 0649

[RequireComponent(typeof(Health), typeof(AudioSource))]
public class PlayerDamageFeedback : MonoBehaviour {

    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;

    new private Camera camera;

	void Start () {

        audioSource = GetComponent<AudioSource>();
        camera = GetComponentInChildren<Camera>();

        var health = GetComponent<Health>();
        health.OnHealthChanged += OnHealthChangedHandler;
	}

    public void OnHealthChangedHandler(Health sender, int oldValue, int newValue) {

        if (newValue >= oldValue) return;

        audioSource.PlayOneShot(audioClip);
        camera.DOShakeRotation(0.5f, strength: 5);
    }
}
