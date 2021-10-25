using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyRemove : MonoBehaviour
{

    [SerializeField] Animator anim = null;
    [SerializeField] float deadBodyRemoveTime = 5f;
    [SerializeField] float deadBodyRemoveSpeed = .25f;
    [SerializeField] float deadBodyunderGroundOffset = .5f;

    Rigidbody rb;
    Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        GetComponent<Entity>().OnDeath += EntityDied;
    }

    void EntityDied(DamageInfo info) 
    {
        if (anim != null) anim.SetTrigger("Death");
        StartCoroutine(BodyRemoveCoroutine());
    }

    IEnumerator BodyRemoveCoroutine()
    {
        col.enabled = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        yield return new WaitForSeconds(deadBodyRemoveTime);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * deadBodyRemoveSpeed;
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down * deadBodyunderGroundOffset, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}
