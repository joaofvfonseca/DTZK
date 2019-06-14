using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_ZombieHealth : MonoBehaviour
{
    private float health;

    private void Start()
    {
    }

    public void SetHealth(float param)
    {
        health = param;
    }

    public void TakeDamage(float param)
    {
        health -= param;
        if ((health <= 0) && GetComponent<Game_ZombieController>().currentState != Game_ZombieController.ZombieState.Dead)
            GetComponent<Game_ZombieController>().currentState = Game_ZombieController.ZombieState.Dying;
    }

    public bool IsZombieDead()
    {
        if (health <= 0) return true;
        return false;
    }
}
