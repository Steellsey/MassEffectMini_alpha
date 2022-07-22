using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private float health;
    public float maxHealth = 1000;
    public GameObject healthBarUI;
    public Slider slider;
    private Camera _cam;

    private void Start() {
        _cam = Camera.main;
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    private void Update() {
        slider.transform.rotation = Quaternion.LookRotation(slider.transform.position - _cam.transform.position);
    }

    public void TakeDamage (int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
        slider.value = CalculateHealth();
        if (health < maxHealth)
            healthBarUI.SetActive(true);
    }

    void Die ()
    {
        Destroy(gameObject);
    }

    float CalculateHealth()
    {
        return health/maxHealth;
    }
}
