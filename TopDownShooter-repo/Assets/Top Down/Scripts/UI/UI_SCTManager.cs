using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_SCTManager : MonoBehaviour
{
    public GameObject SCTPrefab;//This prefab could be stored on the enmies so they can spawn different SCT prefabs
    public GameObject Canvas;



    public void CreateSCT(string SCTText, Transform trans)
    {
       GameObject SCT = Instantiate(SCTPrefab, transform.position, Quaternion.identity);
        SCT.transform.SetParent(Canvas.transform);
        SCT.transform.GetComponentInChildren<Text>().text = SCTText;

        //reposition sct over enemies and player.
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(trans.position);
        SCT.transform.position = screenPosition;
        Destroy(SCT, 0.5f);
    }
}
