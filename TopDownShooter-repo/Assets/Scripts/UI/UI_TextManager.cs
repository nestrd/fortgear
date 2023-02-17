using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_TextManager : MonoBehaviour
{
    public delegate void TooltipEnabled();
    [SerializeField] private UnityEvent OnSpeaking;
    public bool printingDialogue;
    [SerializeField] private string textToPrint;

    protected virtual void Update()
    {
        if (printingDialogue)
        {
            StartCoroutine(nameof(PerformActivation));
        }
        else
        {
            printingDialogue = false;
            CancelInvoke("OnActivated");
        }

    }

    private IEnumerator PerformActivation()
    {
        OnSpeaking?.Invoke();
        yield return null;
    }
}
