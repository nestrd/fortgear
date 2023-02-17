using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVisitCounter : MonoBehaviour
{
    private UI_StatsManager statsRef;

    private void Awake()
    {
        statsRef = FindObjectOfType<UI_StatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            statsRef.UpdateRooms();
            Destroy(this);
        }
    }
}
