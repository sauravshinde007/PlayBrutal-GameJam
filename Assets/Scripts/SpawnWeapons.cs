using UnityEngine;
using Photon.Pun;

public class SpawnWeapons : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // Array of weapon prefabs

    void Start() {
        SpawnWeapon();
    }

    void SpawnWeapon()
    {
        foreach (Transform spawnPoint in transform)
        {
            int randomIndex = Random.Range(0, weaponPrefabs.Length);
            GameObject weaponPrefab = weaponPrefabs[randomIndex];
            PhotonNetwork.Instantiate(weaponPrefab.name, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
