using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIAddText : MonoBehaviour
{

    public Text uiTextPrint;
    [SerializeField] private Animator animRef;
    [SerializeField] private string textToPrint;
    private BoxCollider2D triggerVolume;
    private PlayerController playerRef;

    void Awake()
    {

        playerRef = FindObjectOfType<PlayerController>();
        animRef = GetComponent<Animator>();
        triggerVolume = GetComponent<BoxCollider2D>();

        uiTextPrint.text = textToPrint;
    }

}
