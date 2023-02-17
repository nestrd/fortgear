using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPCBehavior : MonoBehaviour
{

    public Transform target;
    private PlayerController playerRef;
    public GameObject uiTooltip;
    private Animator tipAnim;
    private bool hasSpoken;

    private Text uiTextPrint;
    [SerializeField] private string textToPrint;

    private void Awake()
    {
        playerRef = FindObjectOfType<PlayerController>();
        tipAnim = uiTooltip.GetComponent<Animator>();
        uiTextPrint = uiTooltip.GetComponentInChildren<Text>();

        target = playerRef.gameObject.transform;
    }

    void Update()
    {
        //Always look at player
        Vector3 temp = target.transform.position - transform.position;
        temp.z = 0;
        temp.Normalize();
        transform.up = temp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && hasSpoken == false)
        {
            hasSpoken = true;
            uiTextPrint.text = textToPrint;
            tipAnim.SetTrigger("UITooltipStart");
        }
    }

}
