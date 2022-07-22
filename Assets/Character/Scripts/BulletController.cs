using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private GameObject impactEffect;
    public float speed = 40f;
    public int damage = 40;

    void Start()
    {
        rigidbody.velocity = transform.forward * speed;
        Destroy(gameObject,5f);
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        print("OnTriggerEnter");
        EnemyAI enemy = hitInfo.GetComponent<EnemyAI>();
        if (enemy != null)
            enemy.TakeDamage(damage);
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
