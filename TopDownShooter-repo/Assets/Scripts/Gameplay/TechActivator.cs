using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TechActivator : MonoBehaviour
{
    public AudioSource activationSound;

    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;
    private SpriteRenderer sr;

    public bool activatorOn;

    private void Awake()
    {
        activationSound = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = offSprite;
    }

    public void ActivatorOn()
    {
        sr.sprite = onSprite;
        activationSound.PlayOneShot(activationSound.clip);

    }

}
