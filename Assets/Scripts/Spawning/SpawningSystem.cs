using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

public class SpawningSystem : MonoBehaviour {

    [SerializeField] float waveInterval;
    [SerializeField] int numSpawnersPerWave;
    [SerializeField] Spawner[] spawners;

    Transform playerTransform;
    int currentWaveNumber;

    // Start increasingly large waves at increasing intervals 

    void Start() {

        playerTransform = FindObjectOfType<PlayerController>().transform;

        Invoke("StartNewWave", waveInterval);
    }

    private void StartNewWave() {

        IEnumerable<Spawner> spawnersToUse = spawners
            .OrderBy(GetDistanceToPlayer)
            .Take(numSpawnersPerWave)
            .OrderBy(s => s.numSpawned);

        foreach (Spawner spawner in spawnersToUse) {

            spawner.Spawn();
        }

        currentWaveNumber += 1;
        Invoke("StartNewWave", waveInterval);
    }

    private float GetDistanceToPlayer(Spawner spawner) {

        //throw new NotImplementedException();
        return (spawner.transform.position - playerTransform.position).magnitude;
    }
}
