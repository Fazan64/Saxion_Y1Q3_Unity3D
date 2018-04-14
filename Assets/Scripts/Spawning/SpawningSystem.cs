using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

public class SpawningSystem : MonoBehaviour {

    [SerializeField] float waveInterval = 10f;
    [SerializeField] int firstWaveSize = 3;
    [SerializeField] int podSize = 3;
    [SerializeField] Spawner[] spawners;

    Transform playerTransform;
    int currentWaveNumber;

    int numActiveEnemies;
    int numEnemiesPerWave = 3;

    // Start increasingly large waves at increasing intervals 
    void Start() {

        playerTransform = FindObjectOfType<PlayerController>().transform;

        GlobalEvents.OnEnemyCreated.AddListener(OnEnemyCreated);
        GlobalEvents.OnEnemyDead.AddListener(OnEnemyDead);

        StartCoroutine(StartWaveLoop());
    }

    void OnDestroy() {

        GlobalEvents.OnEnemyCreated.RemoveListener(OnEnemyCreated);
        GlobalEvents.OnEnemyDead.RemoveListener(OnEnemyDead);
    }

    IEnumerator StartWaveLoop() {

        while (true) {

            StartNewWave();
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private void StartNewWave() {

        IEnumerable<Spawner> spawnersToUse = spawners
            .OrderBy(GetDistanceToPlayer);


        numEnemiesPerWave = (int)(Math.Log(currentWaveNumber + 1, 1.5) + firstWaveSize);
        SpawnEnemies(numEnemiesPerWave, spawnersToUse);
        currentWaveNumber += 1;
    }

    private void SpawnEnemies(int numEnemiesToSpawn, IEnumerable<Spawner> spawnersToUse) {

        if (podSize <= 0) return;

        while (true) {

            foreach (Spawner spawner in spawnersToUse) {

                for (int i = 0; i < podSize; ++i) {

                    if (numEnemiesToSpawn <= 0) return;

                    spawner.Spawn();
                    numEnemiesToSpawn -= 1;
                }
            }
        }
    }

    private float GetDistanceToPlayer(Spawner spawner) {

        //throw new NotImplementedException();
        return (spawner.transform.position - playerTransform.position).magnitude;
    }


    private void OnEnemyCreated(GameObject enemy) {

        numActiveEnemies += 1;
    }

    private void OnEnemyDead(GameObject enemy) {

        numActiveEnemies -= 1;
    }
}
