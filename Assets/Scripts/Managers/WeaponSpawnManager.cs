using Photon.Pun;
using UnityEngine;

public class WeaponSpawnManager : MonoBehaviourPun
{
    public GameObject[] weaponPrefabs; // Array of weapon prefabs
    public Transform[] weaponSpawnPoints; // Array of weapon spawn points

    void Start()
    {
        Debug.Log("Start spawning weapons.");
        SpawnWeapons();
    }

    void SpawnWeapons()
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
