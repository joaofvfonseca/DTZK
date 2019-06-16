using UnityEngine;
using TMPro;

public class UI_Ammo : Class_UpdateTextComp
{
    public override void GoAndUpdate()
    {
        param = GameObject.Find("/Player").GetComponent<Game_PlayerWeapon>().GetCurrentEquipedGun().GetComponent<zGame_GunShoot>().GetCurrentAmmo();
        GetComponent<TextMeshProUGUI>().text = string.Format(text, param);
    }
}
