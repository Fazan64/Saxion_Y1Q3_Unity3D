using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 0649

public class TutorialController : MonoBehaviour {

    [SerializeField] float airdropHeight = 100f;
    [SerializeField] float endTutorialDelay = 1f;
    [SerializeField] Airdrop airdropPrefab;
    [SerializeField] PauseScreen pauseScreen;
    [SerializeField] TransitionableScreen tutorialEndScreen;
    [SerializeField] HintTransition[] hints;

    private bool airdropPickedUp;
    private bool isEndingTutorial;

    void Start() {

        Assert.IsNotNull(airdropPrefab);
        Assert.IsNotNull(pauseScreen);
        Assert.IsNotNull(tutorialEndScreen);

        GlobalEvents.OnEnemyDead.AddListener(OnEnemyDead);
    }

    void Update() {

        if (isEndingTutorial) return;

        if (airdropPickedUp && hints.All(hint => !hint.isTransitionedIn)) {
            
            isEndingTutorial = true;
            StartCoroutine(EndTutorial());
        }
    }

    void OnDestroy() {

        GlobalEvents.OnEnemyDead.RemoveListener(OnEnemyDead);
    }

    private void OnEnemyDead(GameObject enemy) {

        Vector3 startPosition = transform.position + Vector3.up * airdropHeight;
        Airdrop airdrop = Instantiate(airdropPrefab, startPosition, Quaternion.identity);

        airdrop.GetComponent<Pickup>().onPickedUp.AddListener(AirdropPickedUpHandler);
    }

    private void AirdropPickedUpHandler(Pickup pickup) {

        airdropPickedUp = true;
    }

    private IEnumerator EndTutorial() {

        yield return new WaitForSeconds(endTutorialDelay);

        pauseScreen.gameObject.SetActive(false);

        PauseController.instance.Pause();
        PauseController.instance.SetCanUnpause(false);

        tutorialEndScreen.TransitionTo();
    }
}
