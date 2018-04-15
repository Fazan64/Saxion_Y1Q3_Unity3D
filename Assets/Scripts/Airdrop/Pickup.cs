using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable 0649

public class Pickup : MonoBehaviour {

    public class PickupEvent : UnityEvent<Pickup> {}

    [SerializeField] int healthIncrement;
    [SerializeField] int scoreIncrement;

    [SerializeField] PickupEvent _onPickedUp = new PickupEvent();
    public PickupEvent onPickedUp { get { return _onPickedUp; } }

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

        onPickedUp.Invoke(this);
        Destroy(gameObject);
    }

    private void PlayPickupEffect() {
        
        //throw new NotImplementedException();
    }
}
