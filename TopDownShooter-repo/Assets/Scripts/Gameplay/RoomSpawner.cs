using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject room;
    private Vector3 offset;
    private BoxCollider2D[] _boxList;

    private void OnTriggerEnter2D(Collider2D roomCol)
    {
        if (roomCol.CompareTag("NExit"))
        {
            offset = roomCol.transform.parent.position + new Vector3(0.0f, 12.5f, 0.0f);
            GameObject temp = Instantiate(room, offset, Quaternion.Euler(Vector3.zero));
            Destroy(roomCol);
            _boxList = temp.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D tempBoxes in _boxList)
            {
                if (tempBoxes.gameObject.name.CompareTo("SExit") == 0)
                {
                    Destroy(tempBoxes);
                }
            }


        }
        if (roomCol.CompareTag("EExit"))
        {
            offset = roomCol.transform.parent.position + new Vector3(14.5f, 0.0f, 0.0f);
            GameObject temp = Instantiate(room, offset, Quaternion.Euler(Vector3.zero));
            Destroy(roomCol);
            _boxList = temp.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D tempBoxes in _boxList)
            {
                if (tempBoxes.gameObject.name.CompareTo("WExit") == 0)
                {
                    Destroy(tempBoxes);
                }
            }
        }
        if (roomCol.CompareTag("SExit"))
        {
            offset = roomCol.transform.parent.position + new Vector3(0.0f, -12.5f, 0.0f);
            GameObject temp = Instantiate(room, offset, Quaternion.Euler(Vector3.zero));
            Destroy(roomCol);
            _boxList = temp.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D tempBoxes in _boxList)
            {
                if (tempBoxes.gameObject.name.CompareTo("NExit") == 0)
                {
                    Destroy(tempBoxes);
                }
            }
        }
        if (roomCol.CompareTag("WExit"))
        {
            offset = roomCol.transform.parent.position + new Vector3(-14.5f, 0.0f, 0.0f);
            GameObject temp = Instantiate(room, offset, Quaternion.Euler(Vector3.zero));
            Destroy(roomCol);
            _boxList = temp.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D tempBoxes in _boxList)
            {
                if (tempBoxes.gameObject.name.CompareTo("EExit") == 0)
                {
                    Destroy(tempBoxes);
                }
            }
        }


    }
}
