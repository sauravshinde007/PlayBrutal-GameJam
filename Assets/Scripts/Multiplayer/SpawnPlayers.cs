using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class SpawnPlayers : MonoBehaviour {


    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineVirtualCamera cinemachineCam;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector2 spawnPoint = GetRandomSpawnPoint();
        var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity).transform;

        cinemachineCam.Follow = player;
        cinemachineCam.LookAt = player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 GetRandomSpawnPoint(){
        if(transform.childCount == 0){
            return transform.position;
        }
        var obj = transform.GetChild(Random.Range(0, transform.childCount));
        var pos = obj.position;
        return pos;
    }
}
