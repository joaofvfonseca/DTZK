using UnityEngine;
using TMPro;

public class UI_RsvAmmo : Class_UpdateTextComp
{
    public override void GoAndUpdate()
    {
        param = GameObject.Find("/Player").GetComponent<Game_PlayerWeapon>().GetCurrentEquipedGun().GetComponent<Game_zGunShoot>().GetCurrentReservAmmo();
        GetComponent<TextMeshProUGUI>().text = string.Format(text, param);
    }
}
