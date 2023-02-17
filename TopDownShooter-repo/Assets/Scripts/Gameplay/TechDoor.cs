using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TechDoor : MonoBehaviour
{

    [HideInInspector] public bool isOpen;
    public bool isActivated;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    public Sprite onSprite;
    public Sprite offSprite;
    public AudioSource activationSound;

    private void Awake()
    {
        activationSound = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        isActivated = false;
    }

    void Update()
    {
        if (isActivated)
        {
            sr.sprite = onSprite;
        }
    }

    public void OpenDoor()
    {
        activationSound.PlayOneShot(activationSound.clip);
        sr.enabled = false;
        col.enabled = false;
    }

}
