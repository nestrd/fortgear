using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechMarker : MonoBehaviour
{
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;
    private SpriteRenderer sr;
    [HideInInspector] public bool isActivated;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ActivateMarker()
    {
        sr.sprite = onSprite;
    }

    public void DeactivateMarker()
    {
        sr.sprite = offSprite;
    }
}
