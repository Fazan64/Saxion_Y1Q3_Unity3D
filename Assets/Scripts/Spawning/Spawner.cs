using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class Spawner : MonoBehaviour {

    [SerializeField] bool autoSpawn;
    [SerializeField][Range(0f, 5f)] float spawnCooldown = 0.1f;
    [SerializeField] float spawnHeight = 50f;
    [SerializeField] float positionRandomizationRadius;
    [SerializeField] float rotationRandomizationAngleDegrees;

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

        Vector3 position = transform.position;
        position += Vector3.up * spawnHeight;

        Vector3 offset = Random.insideUnitCircle * positionRandomizationRadius;
        offset.z = offset.y;
        offset.y = 0f;
        position += offset;

        Quaternion rotation = transform.rotation;
        rotation *= Quaternion.AngleAxis(rotationRandomizationAngleDegrees, Random.onUnitSphere);

        Instantiate(spawnPrefab, position, rotation);

        timeTillCanSpawn += spawnCooldown;
        numSpawned += 1;
    }
}
