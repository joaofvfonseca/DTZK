﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_ManagerUI : MonoBehaviour
{
    [SerializeField] private Class_UpdateTextComp[] comp;

    public enum UIText
    {
        score,
        round,
        ammo,
        rsvAmmo,
        highscore,
        finalScore,
        finalRounds
    }

    private void Start()
    {
        Game_Manager.UITextChange += UpdateUI;
        Game_GunShoot.UITextChange += UpdateUI;
    }

    private void UpdateUI(UIText compToUpdate)
    {
        comp[(int)compToUpdate].GoAndUpdate();
    }
}