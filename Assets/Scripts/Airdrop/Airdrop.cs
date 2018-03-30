using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class Airdrop : MonoBehaviour {

    [SerializeField] ParticleSystem smoke;

    bool didLand;

    void OnCollisionEnter(Collision collision) {

        if (didLand) return;
        didLand = true;

        smoke.Play();
    }
}
