using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainRotation : MonoBehaviour
{

    Quaternion rot;
    Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        rot = Quaternion.identity;
        pos = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localRotation = rot;
        transform.localPosition = pos;
    }
}
