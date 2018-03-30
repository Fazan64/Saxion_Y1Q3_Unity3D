using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

public class AirdropController : MonoBehaviour {

    [SerializeField] float airdropInterval = 10f;
    [SerializeField] float airdropHeight = 100f;
    [SerializeField] Airdrop airdropPrefab;
    [SerializeField] Transform[] airdropTargets;

    int numActiveAirdrops;

    void Start() {

        InitiateAirdrop();
    }

    private void InitiateAirdrop() {

        Transform airdropTarget = GetNextAirdropTarget();

        Vector3 startPosition = airdropTarget.position + Vector3.up * airdropHeight;
        Airdrop drop = Instantiate(airdropPrefab, startPosition, Quaternion.identity);

        Invoke("InitiateAirdrop", airdropInterval);
    }

    private Transform GetNextAirdropTarget() {

        int index = UnityEngine.Random.Range(0, airdropTargets.Length);
        return airdropTargets[index];
    }
}
