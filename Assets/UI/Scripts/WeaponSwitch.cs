using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ShootController;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject weaponSwitchModule;
    [SerializeField] private GameObject weaponSwitchButton;
    private ShootController shootController;
    //private int _weaponType;
    private int currentWeaponSelectedID;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
        shootController = playerObject.GetComponent<ShootController>();
        SwitchWeaponTransfer(1);
    }
    public void togleWeaponSwitchMenu()
    {
        if (!weaponSwitchModule.activeSelf)
        {
            weaponSwitchModule.SetActive(true);
            Time.timeScale = 0.05f;
        }
        else
        {
            weaponSwitchModule.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void SwitchWeaponToPistol()
    {
        SwitchWeaponTransfer(0);
    }
    public void SwitchWeaponToRifle()
    {
        SwitchWeaponTransfer(1);
    }
    public void SwitchWeaponToSpecial()
    {
        SwitchWeaponTransfer(2);
    }
    private void SwitchWeaponTransfer(int newSelectedWeaponID)
    {
        if (newSelectedWeaponID != currentWeaponSelectedID)
        {
            currentWeaponSelectedID = newSelectedWeaponID;
            shootController.SwitchWeapon(newSelectedWeaponID);
            weaponSwitchButton.GetComponentInChildren<Text>().text = shootController._selectedWeapon2[newSelectedWeaponID,0];//---------
        }
        if (weaponSwitchModule.activeSelf)
            togleWeaponSwitchMenu();
    }
}
