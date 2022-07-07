using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecsManager : MonoBehaviour
{
    private float playerHealth;
    public float playerMaxHealth = 100;
        public GameObject playerHealthBarUI;
        public Slider slider;


    void Start()
    {
        playerHealth = playerMaxHealth;
        slider.value = CalculateHealth();
    }

    public void PlayerTakeDamage (int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
            PlayerDie();
        slider.value = CalculateHealth();
        if (playerHealth < playerMaxHealth)
            playerHealthBarUI.SetActive(true);
    }

    void PlayerDie ()
    {
        print("PlayerDead");
    }

    float CalculateHealth()
    {
        return playerHealth/playerMaxHealth;
    }
}
