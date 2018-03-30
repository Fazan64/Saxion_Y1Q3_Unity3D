using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

public class Spawner : MonoBehaviour {
    
    [SerializeField] GameObject spawnPrefab;

    public bool autoSpawn;
    [SerializeField][Range(0f, 20f)] float spawnCooldown = 5f;

    public int numSpawned { get; private set; }

    float timeTillCanSpawn;

    void Start() {

        Debug.Assert(spawnPrefab != null);
    }

    void FixedUpdate() {

        if (!autoSpawn) return;

        if (timeTillCanSpawn > 0f) {
            timeTillCanSpawn -= Time.fixedDeltaTime;
        }

        if (timeTillCanSpawn <= 0f) {
            Spawn();
        }
    }

    public void Spawn() {

        Instantiate(spawnPrefab, transform.position, transform.rotation);
        timeTillCanSpawn = spawnCooldown;

        numSpawned += 1;
    }
}
