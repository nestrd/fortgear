using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public delegate void ActivatorEnabled();
    [SerializeField] private UnityEvent OnActivated;
    public bool activateAll;

    protected virtual void Update()
    {
        if (activateAll)
        {
            StartCoroutine("PerformActivation");
        }
        else
        {
            activateAll = false;
            CancelInvoke("OnActivated");
        }

    }

    private IEnumerator PerformActivation()
    {
        OnActivated?.Invoke();
        yield return null;
    }
}
