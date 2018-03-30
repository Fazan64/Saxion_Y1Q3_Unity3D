using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class Pickup : MonoBehaviour {

    [SerializeField] int healthIncrement;
    [SerializeField] int scoreIncrement;

    int playerLayer;

    void Start() {

        playerLayer = LayerMask.NameToLayer("Player");
    }

    void OnCollisionEnter(Collision collision) {

        GameObject collidee = collision.gameObject;
        if (collidee.layer != playerLayer) return;

        if (healthIncrement > 0) {
            collidee.GetComponent<Health>().Increase(healthIncrement);
        }

        if (scoreIncrement > 0) {
            collidee.GetComponent<Player>().score += scoreIncrement; 
        }

        PlayPickupEffect();
        Destroy(gameObject);
    }

    private void PlayPickupEffect() {
        
        //throw new NotImplementedException();
    }
}
