using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

public class Spawner : MonoBehaviour {

    [SerializeField] bool autoSpawn;
    [SerializeField][Range(0f, 5f)] float spawnCooldown = 0.1f;

    [SerializeField] GameObject spawnPrefab;

    public int numSpawned { get; private set; }

    int numLeftToSpawn;
    float timeTillCanSpawn;

    void Start() {

        Debug.Assert(spawnPrefab != null);
    }

    void FixedUpdate() {

        if (timeTillCanSpawn > 0f) {
            timeTillCanSpawn -= Time.fixedDeltaTime;
        }

        if (autoSpawn || numLeftToSpawn > 0) {

            if (timeTillCanSpawn <= 0f) {

                numLeftToSpawn -= 1;
                SpawnImmediate();
            }
        }
    }

    public void Spawn() {

        numLeftToSpawn += 1;
    }

    public void SpawnImmediate() {

        Instantiate(spawnPrefab, transform.position + Vector3.up * 50f, transform.rotation);

        timeTillCanSpawn += spawnCooldown;
        numSpawned += 1;
    }
}
