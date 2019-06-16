using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Game_PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject interactString;

    private bool shotgunUnlocked;
    private bool rifleUnlocked;

    private GameObject gunObj;
    private string initialInteract;

    private enum SelectedGun
    {
        pistol,
        shotgun,
        rifle,
        none
    }
    private SelectedGun currentlySelectedGun;
    private SelectedGun equippedGun;

    private Game_Manager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        equippedGun = SelectedGun.pistol;
        gunObj = pistol;
        currentlySelectedGun = SelectedGun.none;
        initialInteract = interactString.GetComponent<TextMeshProUGUI>().text;
        gameManager = GameObject.Find("/!MANAGER").GetComponent<Game_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        GrabFromWall();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "WallGun") return;
        if (collision.gameObject.name == "Pistol") currentlySelectedGun = SelectedGun.pistol;
        if (collision.gameObject.name == "M4_Carbine") currentlySelectedGun = SelectedGun.rifle;
        if (collision.gameObject.name == "870_Shotgun") currentlySelectedGun = SelectedGun.shotgun;

        if (currentlySelectedGun == equippedGun) interactString.GetComponent<TextMeshProUGUI>().text = initialInteract + "buy ammunition for 500 points";
        if (currentlySelectedGun != equippedGun) interactString.GetComponent<TextMeshProUGUI>().text = initialInteract + "change weapon\nand buy ammunition for 1500 points";
        interactString.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "WallGun") return;
        currentlySelectedGun = SelectedGun.none;
        interactString.SetActive(false);
    }

    private void GrabFromWall()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (equippedGun == currentlySelectedGun)
            {
                if (!gameManager.HasEnoughPoints(500)) return;
                AddAmmo();
                gameManager.RemoveScore(500);
            }
            if (equippedGun != currentlySelectedGun)
            {
                if (!gameManager.HasEnoughPoints(1500)) return;
                equippedGun = currentlySelectedGun;
                ChangeActualGun();
                gameManager.RemoveScore(1500);
            }
        }
    }

    private void AddAmmo()
    {
        gunObj.GetComponent<Game_zGunShoot>().AddAmmoToReserve();
    }

    private void ChangeActualGun()
    {
        if(equippedGun == SelectedGun.pistol)
        {
            pistol.SetActive(true);
            rifle.SetActive(false);
            shotgun.SetActive(false);

            gunObj = pistol;
        }
        if (equippedGun == SelectedGun.rifle)
        {
            pistol.SetActive(false);
            rifle.SetActive(true);
            shotgun.SetActive(false);

            gunObj = rifle;
        }
        if (equippedGun == SelectedGun.shotgun)
        {
            pistol.SetActive(false);
            rifle.SetActive(false);
            shotgun.SetActive(true);

            gunObj = shotgun;
        }
    }

    public GameObject GetCurrentEquipedGun()
    {
        return gunObj;
    }
}
