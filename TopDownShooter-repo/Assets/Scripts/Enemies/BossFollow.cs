using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollow : MonoBehaviour
{
    public GameObject playerRef;

    void Update()
    {
        transform.position = new Vector2(playerRef.transform.position.x, transform.position.y);
    }
}
