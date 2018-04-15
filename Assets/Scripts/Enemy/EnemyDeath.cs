using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 0649

[RequireComponent(typeof(Health), typeof(Enemy))]
public class EnemyDeath : MonoBehaviour {

    [SerializeField] ScoreBonusText bonusTextPrefab;

    private Enemy enemy;

    void Start () {

        Assert.IsNotNull(bonusTextPrefab);

        enemy = GetComponent<Enemy>();
        GetComponent<Health>().OnDeath += OnDeathHandler;
	}

    private void OnDeathHandler(Health sender) {

        enemy.fsm.ChangeState<EnemyDeadState>();
        CreateBonusText();
    }

    private void CreateBonusText() {

        ScoreBonusText bonusText = Instantiate(bonusTextPrefab);
        bonusText.transform.position = transform.position;
    }
}
