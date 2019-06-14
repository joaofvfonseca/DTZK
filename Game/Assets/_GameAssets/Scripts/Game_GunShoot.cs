using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Game_GunShoot : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactRock;
    [SerializeField] private GameObject impactZombie;

    [SerializeField] private float baseDamage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;

    [SerializeField] private int maxAmmo;
    [SerializeField] private int maxReservAmmo;

    [SerializeField] private float reloadTime;
    [SerializeField] private Animator animator;

    [SerializeField] private AudioSource NoAmmo;


    private Game_Manager gameManager;
    private AudioSource shotSound;

    private int currentAmmo;
    private int currentReservAmmo;
    private bool isReloading;

    private float nextFire;

    private void Start()
    {
        gameManager = GameObject.Find("/!MANAGER").GetComponent<Game_Manager>();
        shotSound = GetComponent<AudioSource>();

        nextFire = 0;

        currentAmmo = maxAmmo;
        currentReservAmmo = maxReservAmmo;
        isReloading = false;
    }

    private void OnEnable()
    {
        nextFire = 0;

        currentAmmo = maxAmmo;
        currentReservAmmo = maxReservAmmo;
        isReloading = false;
        animator.SetBool("reloading", false);
    }

    // Update is called once per frame
    private void Update()
    {
        Shoot();
        StartCoroutine(Reload());
    }

    private void Shoot()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + 1 / fireRate;
            if (!HasAmmo())
            {
                NoAmmo.Play();
                return;
            }
            if (isReloading) return;
            currentAmmo -= 1;
            shotSound.Play();
            muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                Game_ZombieHealth zombie = hit.transform.root.GetComponent<Game_ZombieHealth>();
                string zombieHit = hit.transform.name;
                if (zombie)
                {
                    ShotOnZombie(hit, zombie, zombieHit);
                }
                else
                {
                    Instantiate(impactRock, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }

    private void ShotOnZombie(RaycastHit hit, Game_ZombieHealth zombie, string zombieHit)
    {
        if (zombieHit == "Bone01")
        {
            zombie.TakeDamage(baseDamage * 15);
            gameManager.AddToScoreQueue(Class_Score.ScoreID.headshot, 10, "Headshot");
        }
        else if (zombieHit == "Bip01 Pelvis")
        {
            zombie.TakeDamage(baseDamage * 3.5f);
            gameManager.AddToScoreQueue(Class_Score.ScoreID.chest_shot, 6, "Torso hit");
        }
        else
        {
            zombie.TakeDamage(baseDamage);
            gameManager.AddToScoreQueue(Class_Score.ScoreID.limb_shot, 4, "Limb hit");
        }
        gameManager.AddToScoreQueue(Class_Score.ScoreID.bullet_hit, 1, "Shot on target");
        Instantiate(impactZombie, hit.point, Quaternion.LookRotation(hit.normal));
    }

    private IEnumerator Reload()
    {
        if (Input.GetButtonDown("Reload") && !isReloading)
        {
            if (currentReservAmmo <= 0 || currentAmmo == maxAmmo) yield break;
            isReloading = true;
            animator.SetBool("reloading", true);
            yield return new WaitForSeconds(reloadTime - 0.25f);
            int neededAmmo = maxAmmo - currentAmmo;
            if (neededAmmo <= currentReservAmmo)
            {
                currentReservAmmo -= neededAmmo;
                currentAmmo += neededAmmo;
            }
            else
            {
                currentAmmo += currentReservAmmo;
                currentReservAmmo = 0;
            }
            animator.SetBool("reloading", false);
            yield return new WaitForSeconds(0.25f);
            isReloading = false;
        }
    }

    private bool HasAmmo()
    {
        if (currentAmmo > 0) return true;
        return false;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetCurrentReservAmmo()
    {
        return currentReservAmmo;
    }

    public void AddAmmoToReserve()
    {
        currentReservAmmo = maxReservAmmo;
    }
}
