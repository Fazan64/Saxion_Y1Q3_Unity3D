using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

public class SpawningSystem : MonoBehaviour {

    [SerializeField] float waveInterval = 10f;
    [SerializeField] float targetNumEnemiesIncreaseInterval = 5f;
    [SerializeField] int numSpawnersPerWave = 3;
    [SerializeField] Spawner[] spawners;

    Transform playerTransform;
    int currentWaveNumber;

    int numActiveEnemies;
    int numEnemiesPerWave = 3;

    // Start increasingly large waves at increasing intervals 
    void Start() {

        playerTransform = FindObjectOfType<PlayerController>().transform;

        GlobalEvents.OnEnemyCreated.AddListener(enemy => numActiveEnemies += 1);
        GlobalEvents.OnEnemyDead.AddListener(enemy => numActiveEnemies -= 1);

        StartCoroutine(StartWaveLoop());
        StartCoroutine(TargetNumEnemiesLoop());
    }

    IEnumerator TargetNumEnemiesLoop() {

        while (true) {

            numEnemiesPerWave += 1;
            yield return new WaitForSeconds(targetNumEnemiesIncreaseInterval);
        }
    }

    IEnumerator StartWaveLoop() {

        while (true) {

            StartNewWave();
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private void StartNewWave() {

        IEnumerable<Spawner> spawnersToUse = spawners
            .OrderBy(GetDistanceToPlayer)
            .Take(numSpawnersPerWave)
            .OrderBy(s => s.numSpawned);

        SpawnEnemies(numEnemiesPerWave, spawnersToUse);
        currentWaveNumber += 1;
    }

    private void SpawnEnemies(int numEnemiesToSpawn, IEnumerable<Spawner> spawnersToUse) {

        while (true) {

            foreach (Spawner spawner in spawnersToUse) {

                if (numEnemiesToSpawn <= 0) return;

                spawner.Spawn();
                numEnemiesToSpawn -= 1;
            }
        }
    }

    private float GetDistanceToPlayer(Spawner spawner) {

        //throw new NotImplementedException();
        return (spawner.transform.position - playerTransform.position).magnitude;
    }
}
