using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Ui_Game : UiBase
{
    [SerializeField] private TextMeshProUGUI turnText;

    [SerializeField] private GameObject dynamicModeGo;

    public override void HideUi()
    {
        transform.DOScale(0, 0);
        DynamicMode(false);
    }


    public override void ShowUi()
    {
        transform.DOScale(1, .2f);
        SetActivePlayerText(GameManager.I.activeTeam);
        DynamicMode(false);
    }

    public override void SetActivePlayerText(TeamColor activeTeam)
    {
        turnText.text = activeTeam == TeamColor.White ? "You" : "Opponent";
    }

    public void DynamicMode(bool entered)
    {
        dynamicModeGo.SetActive(entered);
    }
}