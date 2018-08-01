using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 0649

public class Airdrop : MonoBehaviour {

    public event Action<Airdrop> OnDestroyed;

    [SerializeField] ParticleSystem smoke;

    private bool didLand;

    void Start() {

        Assert.IsNotNull(smoke);
    }

    void OnCollisionEnter(Collision collision) {

        if (didLand) return;
        didLand = true;

        smoke.Play();
    }

    void OnDestroy() {

        if (OnDestroyed != null) {
            OnDestroyed(this);
        }
    }
}
