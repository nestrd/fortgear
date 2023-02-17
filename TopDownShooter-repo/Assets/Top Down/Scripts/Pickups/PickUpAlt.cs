using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAlt : MonoBehaviour
{
    public WeaponAlt weapon;
    public float timeToAppear;
    public float despawnTime;
    public ParticleSystem pickUpFX;
    public Sprite pickUpSprite;

    protected SpriteRenderer spriteRenderer;
    protected Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (pickUpSprite == null)
        {
            SpriteRenderer weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();

            if (weaponSprite != null)
            {
                spriteRenderer.sprite = weapon.GetComponentInChildren<SpriteRenderer>().sprite;
            }
        }
        else
        {
            spriteRenderer.sprite = pickUpSprite;
        }

        spriteRenderer.enabled = false;
        col.enabled = false;
    }

    void Start()
    {
        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        yield return new WaitForSeconds(timeToAppear);

        StartCoroutine(Despawn());
        spriteRenderer.enabled = true;
        col.enabled = true;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);

        Destroy(gameObject);
    }

    /*
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }*/

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerControllerAlt controller = other.GetComponent<PlayerControllerAlt>();

        if (controller != null)
        {
            WeaponAlt weaponClone = Instantiate(weapon);
            weaponClone.name = weapon.name;

            controller.EquipWeapon(weaponClone);

            if (pickUpFX != null)
            {
                Instantiate(pickUpFX, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
