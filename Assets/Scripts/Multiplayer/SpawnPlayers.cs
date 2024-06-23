using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector2 spawnPoint = GetRandomSpawnPoint();
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 GetRandomSpawnPoint(){
        if(transform.childCount == 0){
            return transform.position;
        }
        return transform.GetChild(Random.Range(0, transform.childCount)).position;
    }
}
