using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_PlayerHealth : MonoBehaviour
{
    private float health;
    private bool damaged5SecondsAgo;

    [SerializeField] private Image damageIndicatorImage;
    private Color startColor;

    private void Start()
    {
        health = 100;
        damaged5SecondsAgo = false;
        startColor = damageIndicatorImage.color;   
    }

    private void Update()
    {
        if ((health < 100) && (health > 0)) StartCoroutine(RestoreHealth());
        if (health > 100) health = 100;

        startColor.a = 1 - (health * 0.01f);
        damageIndicatorImage.color = startColor;
    }

    public IEnumerator TakeDamage(float param)
    {
        GetComponent<AudioSource>().Play();
        health -= param;
        damaged5SecondsAgo = true;
        float tempHealth = health;
        yield return new WaitForSeconds(8);
        if (tempHealth != health) yield break;
        damaged5SecondsAgo = false;
    }

    private IEnumerator RestoreHealth()
    {
        if (damaged5SecondsAgo) yield break;
        health += 0.05f;
    }

    public bool isPlayerDead()
    {
        if (health > 0) return false;
        return true;
    }
}
