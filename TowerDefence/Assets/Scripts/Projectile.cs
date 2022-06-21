using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    float speed = 20;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase temp;
        if(other.TryGetComponent<EnemyBase>(out temp))
        {
            temp.Damage(damage);
            Hit();
        }
        Debug.Log("Triggered");
    }

    void Hit()
    {
        Destroy(gameObject);
        //Implement Explosion sound and particle effect
    }
}
