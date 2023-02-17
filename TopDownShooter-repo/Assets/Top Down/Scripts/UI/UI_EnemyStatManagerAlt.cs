using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyStatManagerAlt : MonoBehaviour
{
    protected ControllerAlt controller;
    protected Image healthBarImage;
    public GameObject EnemyCanvas;



    private void Awake()
    {
        //bug in prefab editor, this is a work around.
        GameObject canvas = Instantiate(EnemyCanvas, Vector3.zero, Quaternion.identity);
        canvas.transform.SetParent(transform);
        canvas.GetComponent<RectTransform>().localPosition = new Vector3(0, 0.65f, 0);
        controller = GetComponentInParent<ControllerAlt>();


        //Not the best way to get reference to our Health Image.
        healthBarImage = (Image)canvas.transform.Find("HealthBarBG/HealthBar").GetComponent<Image>();
    }


    // Update is called once per frame
    void Update()
    {
        healthBarImage.fillAmount = controller.GetHealth() / controller.maxHealth;
    }
}
