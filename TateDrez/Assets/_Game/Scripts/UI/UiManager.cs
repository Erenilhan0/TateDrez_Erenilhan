using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager I;
    
    public UiBase[] uis;
    
    private void Awake()
    {
        I = this;
    }

    private void OnOnGamePhaseChange(GamePhase obj)
    {
        for (int i = 0; i < uis.Length; i++)
        {
            if ((int)obj == i)
            {
                uis[i].ShowUi();
            }
            else
            {
                uis[i].HideUi();
            }
        }

        switch (obj)
        {
            case GamePhase.Dice:
                break;
            case GamePhase.Place:
                break;
            case GamePhase.Game:
                break;
            case GamePhase.End:
                break;
        }
    }


    private void Start()
    {
        GameManager.I.OnGamePhaseChange += OnOnGamePhaseChange;
    }


    public void OnClickNextLevel()
    {
        GameManager.I.OnClickNextLevel();
    }

    public void OnClickRestart()
    {
        GameManager.I.OnClickRestart();
    }

    
}