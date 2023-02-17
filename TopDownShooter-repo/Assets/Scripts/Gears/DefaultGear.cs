using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGear : PlayerGear
{

    public override void OnThrown()
    {
        StartCoroutine("ThrowingCog");
    }
    
    public IEnumerator ThrowingCog()
    {
        cogAudio.Play();
        cogRb.simulated = true;
        cogAnim.SetBool("IsThrowing", true);
        uiAnim.SetBool("IsThrowing", true);
        float t = 0.0f;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.SetParent(null);
        cogRb.bodyType = RigidbodyType2D.Dynamic;
        while (t <= travelDistance)
        {
            cogRb.AddForce(playerRb.gameObject.transform.up * cogSpeed, ForceMode2D.Impulse);
            t += Time.deltaTime;
            yield return null;
        }
        StopCoroutine("ThrowingCog");
        StartCoroutine("ReturningCog");
        yield break;
    }

    public IEnumerator ReturningCog()
    {
        uiAnim.SetBool("IsReturning", true);
        cogAnim.SetBool("IsReturning", true);
        float t = 0.0f;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        cogRb.velocity = Vector3.zero;
        while (t <= returnTime) 
        {
            Vector3 resetPos = FindObjectOfType<PlayerController>().cogOrigin.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, resetPos, t * returnTime);
            t += Time.deltaTime;
            yield return null;
        }
        transform.SetParent(FindObjectOfType<PlayerController>().gameObject.transform);
        cogAnim.SetBool("IsThrowing", false);
        cogAnim.SetBool("IsReturning", false);
        uiAnim.SetBool("IsThrowing", false);
        uiAnim.SetBool("IsReturning", false);
        GetComponentInParent<PlayerController>().isThrowing = false;

        _cogManager.SwitchState(typeof(HeldState));
        StopCoroutine("ReturningCog");
        yield break;
    }

    
}
