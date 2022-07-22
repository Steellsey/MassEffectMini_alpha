using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class ShootController : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform viewPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float _raycastShootDistance = 25f;
    [SerializeField] GameObject bulletsCountUI;
    [SerializeField] GameObject aimLazer;

    [Header("ShootSounds")]
    [SerializeField] AudioSource _rifleWeaponSound;
    [SerializeField] AudioSource _pistolWeaponSound;
    [SerializeField] AudioSource _specialWeaponSound;
    [SerializeField] AudioSource _reloadSound;
    private bool _isReadyToShot;
    private bool  _isResetShotInProcess;
    private float timeBetweenShooting;
    private bool isReloading;
    private int _bulletsCount;
    private int _bulletsCountMax;
    private float reloadTime;
    private int selectedWeaponID;
    private int weaponDamage;
    private string weaponShootType;
    private string weaponType;
    public string[,] _selectedWeapon2 = new string [3,7]
    {
        //Name, Type, HitDamage, TimeBetweenShooting, BulletsInFirearm, ReloadTime
        {"Glock", "rayCastWeapon", "40", "0.3", "9", "0.5", "Pistol"},
        {"M4", "rayCastWeapon", "45", "0.12", "30", "1.2", "Rifle"},
        {"Grenader", "physicWeapon", "550", "0", "1", "1.5", "Special"}
    };

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _isReadyToShot = true;
    }
    private void start()
    {
        _rifleWeaponSound = GetComponent<AudioSource>();
        _pistolWeaponSound = GetComponent<AudioSource>();
        _specialWeaponSound = GetComponent<AudioSource>();
        _reloadSound = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        var viewVector = new Vector3(-_input.ViewVector.y, 0, _input.ViewVector.x);
        if (viewVector != Vector3.zero)
        {
            aimLazer.SetActive(true);
            RaycastHit aimInfo;
            if (Physics.Raycast(viewPoint.position, viewPoint.forward, out aimInfo, _raycastShootDistance))
            {
                EnemyAI AimingToEnemy = aimInfo.transform.GetComponent<EnemyAI>();
                if (AimingToEnemy != null)
                {
                    if (_isReadyToShot)
                    {
                        if (_bulletsCount > 0)
                        {
                            Shoot();
                            _isReadyToShot = false;
                            if (!_isResetShotInProcess)
                            {
                                StartCoroutine(ResetShoot());
                                _isResetShotInProcess = true;
                            }
                        }
                        else
                            StartCoroutine(ReloadWeapon());
                    }
                }
            }
        }
        else aimLazer.SetActive(false);
    }
    private IEnumerator ResetShoot()
    {
        yield return new WaitForSecondsRealtime(timeBetweenShooting);
        _isReadyToShot = true;
        _isResetShotInProcess = false;
    }
    private void Shoot()
    {
        if (weaponShootType == "physicWeapon")
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<BulletController>().damage = weaponDamage;//---------
        }
        else if (weaponShootType == "rayCastWeapon")
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hitInfo, _raycastShootDistance))
            {
                EnemyAI enemy = hitInfo.transform.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(weaponDamage);
                }
                Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            }
        }
        ShootSound();
        _bulletsCount --;
        bulletsCountUI.GetComponent<Text>().text = _bulletsCount.ToString();//---------
    }
    private void ShootSound()//---------
    {
        if (weaponType == _selectedWeapon2[0,6])
            _pistolWeaponSound.Play();
        else if (weaponType == _selectedWeapon2[1,6])
            _rifleWeaponSound.Play();
        else if (weaponType == _selectedWeapon2[2,6])
            _specialWeaponSound.Play();
    }
    public void SwitchWeapon(int newSelectedWeaponID)
    {
        //Смена модели оружия
        selectedWeaponID = newSelectedWeaponID;
        weaponShootType = _selectedWeapon2[selectedWeaponID,1];
        weaponDamage = int.Parse(_selectedWeapon2[selectedWeaponID,2]);
        timeBetweenShooting = float.Parse(_selectedWeapon2[selectedWeaponID,3], CultureInfo.InvariantCulture.NumberFormat);
        _bulletsCountMax  = int.Parse(_selectedWeapon2[selectedWeaponID,4]);
        reloadTime = float.Parse(_selectedWeapon2[selectedWeaponID,5], CultureInfo.InvariantCulture.NumberFormat);
        weaponType = _selectedWeapon2[selectedWeaponID,6];
        _bulletsCount = _bulletsCountMax;
        bulletsCountUI.GetComponent<Text>().text = _bulletsCount.ToString();
    }
    public void ReloadWeaponButton()
    {
        StartCoroutine(ReloadWeapon());
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
}