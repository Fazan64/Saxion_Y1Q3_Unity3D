using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject spawnPrefab;

    [Range(0f, 20f)] public float spawnCooldown = 5f;
    float timeTillCanSpawn;

    void Start() {

        Debug.Assert(spawnPrefab != null);
    }

    void FixedUpdate() {

        if (timeTillCanSpawn > 0f) {
            timeTillCanSpawn -= Time.fixedDeltaTime;
        }

        if (timeTillCanSpawn <= 0f) {
            Spawn();
        }
    }

    private void Spawn() {

        Instantiate(spawnPrefab, transform.position, transform.rotation);
        timeTillCanSpawn = spawnCooldown;
    }
}
