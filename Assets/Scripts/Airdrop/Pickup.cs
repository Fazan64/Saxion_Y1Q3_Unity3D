using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

#pragma warning disable 0649

public class Pickup : MonoBehaviour {

    public class PickupEvent : UnityEvent<Pickup> {}

    [SerializeField] TriggerEvents trigger;
    [SerializeField] ScoreBonusText bonusTextPrefab;
    [SerializeField] float awayFromPlayerBonusTextDisplacement = 1f;
    [SerializeField] int healthIncrement;
    [SerializeField] int scoreIncrement;

    [SerializeField] PickupEvent _onPickedUp = new PickupEvent();
    public PickupEvent onPickedUp { get { return _onPickedUp; } }

    int playerLayer;

    void Start() {

        Assert.IsNotNull(trigger);
        Assert.IsNotNull(bonusTextPrefab);

        trigger.onPlayerTriggerEnter.AddListener(OnPlayerTriggerEnter);
    }

    void OnDestroy() {

        trigger.onPlayerTriggerEnter.RemoveListener(OnPlayerTriggerEnter);
    }

    private void OnPlayerTriggerEnter() {

        Player player = Player.instance;

        if (healthIncrement > 0) {
            player.health.Increase(healthIncrement);
        }

        if (scoreIncrement > 0) {
            player.score += scoreIncrement; 
        }

        PlayPickupEffect();

        onPickedUp.Invoke(this);
        Destroy(gameObject);
    }

    private void PlayPickupEffect() {

        if (scoreIncrement > 0) {
            
            ScoreBonusText bonusText = Instantiate(bonusTextPrefab);
            bonusText.SetText("+" + scoreIncrement);

            Vector3 awayFromPlayer = transform.position - Player.instance.transform.position;
            awayFromPlayer = Vector3.ProjectOnPlane(awayFromPlayer, Vector3.up).normalized;
            bonusText.transform.position = transform.position + awayFromPlayer * awayFromPlayerBonusTextDisplacement;
        }
    }
}
