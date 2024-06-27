using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnWeapons : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // Array of weapon prefabs
    public Transform[] weaponSpawnPoints; // Array of weapon spawn points

    void Start()
    {
        Debug.Log("Start spawning weapons.");
        SpawnWeapon();
    }

    void SpawnWeapon()
    {
        foreach (Transform spawnPoint in weaponSpawnPoints)
        {
            int randomIndex = Random.Range(0, weaponPrefabs.Length);
            GameObject weaponPrefab = weaponPrefabs[randomIndex];
            Debug.Log($"Spawning {weaponPrefab.name} at {spawnPoint.position}");
            PhotonNetwork.Instantiate(weaponPrefab.name, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
