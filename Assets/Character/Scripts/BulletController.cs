using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 40f;
    public int damage = 40;
    public Rigidbody rigidbody;
    public GameObject impactEffect;

    void Start()
    {
        rigidbody.velocity = transform.forward * speed;
        Destroy(gameObject,5f);
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        print("OnTriggerEnter");
        EnemyController enemy = hitInfo.GetComponent<EnemyController>();
        if (enemy != null)
            enemy.TakeDamage(damage);
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
