using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSpecsManager : MonoBehaviour
{
    [SerializeField] private float _playerMaxHealth = 1000f;
    [SerializeField] private GameObject _playerHealthBarUI;
    [SerializeField] private Slider _playerHealthBarSlider;
    private float _playerCurrentHealth;

    public void PlayerTakeDamage (float damage)
    {
        _playerCurrentHealth -= damage;
        if (_playerCurrentHealth <= 0)
            KillPlayer();
        _playerHealthBarSlider.value = CalculateHealth();
        if (_playerCurrentHealth < _playerMaxHealth)
            _playerHealthBarUI.SetActive(true);
    }
    private void Start()
    {
        _playerCurrentHealth = _playerMaxHealth;
        _playerHealthBarSlider.value = CalculateHealth();
    }
    private void KillPlayer ()
    {
        print("PlayerDead");
        SceneManager.LoadScene(0);//---------
    }
    private float CalculateHealth()
    {
        return _playerCurrentHealth/_playerMaxHealth;
    }
}
