using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ShootController;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject playerObject;
    private ShootController shootController;
    public GameObject weaponSwitchModule;
    public GameObject weaponSwitchButton;
    private int _weaponType;

    void Awake()
    {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
        shootController = playerObject.GetComponent<ShootController>();
        SwitchWeaponTransfer(1);
    }

    public void togleWeaponSwitchMenu()
    {
        if (!weaponSwitchModule.active)
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
        shootController.SwitchWeapon(newSelectedWeaponID);
        weaponSwitchButton.GetComponentInChildren<Text>().text = shootController._selectedWeapon2[newSelectedWeaponID,0];
        if (weaponSwitchModule.active)
            togleWeaponSwitchMenu();
    }
}
