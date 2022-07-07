using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class ShootController : MonoBehaviour
{
    private InputHandler _input;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    public float _raycastShootDistance = 100f;

    private bool readyToShoot, isCoroutineExecuting;
    private float timeBetweenShooting;
    private float reloadTime;
    private bool isReloading;
    private int _bulletsCount;
     private int _bulletsCountMax;   
    public GameObject bulletsCountUI;
    public GameObject aimLazer;
    public AudioSource _rifleWeaponSound;
    public AudioSource _pistolWeaponSound;
    public AudioSource _specialWeaponSound;
    public AudioSource _reloadSound;
    int selectedWeaponID;
    int weaponDamage;
    string weaponShootType;
    string weaponType;
    public string[,] _selectedWeapon2 = new string [3,7]
    {
        //Name, Type, HitDamage, TimeBetweenShooting, BulletsInFirearm, ReloadTime
        {"Glock", "rayCastWeapon", "55", "0.3", "9", "0.5", "Pistol"},
        {"M4", "rayCastWeapon", "60", "0.12", "30", "1.2", "Rifle"},
        {"Grenader", "physicWeapon", "550", "0", "1", "1.5", "Special"}
    };

    void start()
    {
        _rifleWeaponSound = GetComponent<AudioSource>();
        _pistolWeaponSound = GetComponent<AudioSource>();
        _specialWeaponSound = GetComponent<AudioSource>();
        _reloadSound = GetComponent<AudioSource>();
    }

    private void Awake() {
        _input = GetComponent<InputHandler>();
        readyToShoot = true;
        //SwitchWeapon(1);
    }

    void FixedUpdate()
    {
        //=====
        var viewVector = new Vector3(-_input.ViewVector.y, 0, _input.ViewVector.x);
        if (viewVector != Vector3.zero)
        {
            aimLazer.SetActive(true);
            RaycastHit aimInfo;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out aimInfo, _raycastShootDistance))
            {
                EnemyController AimingToEnemy = aimInfo.transform.GetComponent<EnemyController>();
                if (AimingToEnemy != null)
                {
                    Invoke("ShootWithDelay", 0.15f);//---------
                    //ShootWithDelay();
                }
            }
        }
        else aimLazer.SetActive(false);
    }

    void ShootWithDelay()
    {
        if (readyToShoot)
            {
                if (_bulletsCount > 0)
                {
                    Shoot();
                    readyToShoot = false;
                    if (!isCoroutineExecuting)
                    {
                        //StartCoroutine(ResetShoot(timeBetweenShooting));
                        Invoke("ResetShoot", timeBetweenShooting);
                        //Invoke("ResetShoot", 0.2f);
                        isCoroutineExecuting = true;
                    }
                }
                else
                    StartCoroutine(ReloadWeapon());
            }
    }

    //IEnumerator ResetShoot(float waitTime)
    void ResetShoot()
    {
        //isCoroutineExecuting = true;
        //yield return new WaitForSeconds(waitTime);
        //yield return new WaitForSecondsRealtime(waitTime);
        readyToShoot = true;
        isCoroutineExecuting = false;
    }

    void Shoot()
    {
        if (weaponShootType == "physicWeapon")
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<BulletController>().damage = weaponDamage;//----Сильно загружает, надо переделать
        }
        else if (weaponShootType == "rayCastWeapon")
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hitInfo, _raycastShootDistance))
            {
                EnemyController enemy = hitInfo.transform.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(weaponDamage);
                }
                Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            }
        }
        ShootSound();
        _bulletsCount --;
        bulletsCountUI.GetComponent<Text>().text = _bulletsCount.ToString();
    }

    private void ShootSound()
    {
        print (_selectedWeapon2[1,0]);
        if (weaponType == _selectedWeapon2[0,6])
            _pistolWeaponSound.Play();
        else if (weaponType == _selectedWeapon2[1,6])
            _rifleWeaponSound.Play();
        else if (weaponType == _selectedWeapon2[2,6])
            _specialWeaponSound.Play();

    }

    public void SwitchWeapon(int newSelectedWeaponID)
    {
        selectedWeaponID = newSelectedWeaponID;
        //Смена модели оружия
        weaponShootType = _selectedWeapon2[selectedWeaponID,1];
        weaponType = _selectedWeapon2[selectedWeaponID,6];
        timeBetweenShooting = float.Parse(_selectedWeapon2[selectedWeaponID,3], CultureInfo.InvariantCulture.NumberFormat);
        _bulletsCountMax  = int.Parse(_selectedWeapon2[selectedWeaponID,4]);
        _bulletsCount = _bulletsCountMax;
        bulletsCountUI.GetComponent<Text>().text = _bulletsCount.ToString();
        reloadTime = float.Parse(_selectedWeapon2[selectedWeaponID,5], CultureInfo.InvariantCulture.NumberFormat);
        weaponDamage = int.Parse(_selectedWeapon2[selectedWeaponID,2]);
    }

    public IEnumerator ReloadWeapon()
    {
        if (!isReloading && _bulletsCount < _bulletsCountMax)
            {
                isReloading = true;
                _reloadSound.Play();
                yield return new WaitForSeconds(reloadTime);
                _bulletsCount = _bulletsCountMax;
                bulletsCountUI.GetComponent<Text>().text = _bulletsCount.ToString();
                isReloading = false;
            }
    }

    public void ReloadWeaponButton()
    {
        StartCoroutine(ReloadWeapon());
    }
}