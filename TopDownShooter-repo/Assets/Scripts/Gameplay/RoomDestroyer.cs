using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDestroyer : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.CompareTag("RoomCounter"))
        {
            Destroy(this);
        }
    }

}
