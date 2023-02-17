using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    public GameObject connectingDoor;
    public Transform offset;

    private void Awake()
    {
        //offset = GetComponentInChildren<Transform>();
    }
}
