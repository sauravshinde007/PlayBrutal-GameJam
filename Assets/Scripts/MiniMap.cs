using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        player = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        Vector3 newposition = player.position;
        newposition.y = transform.position.y;
        transform.position = newposition;


    }
}