using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Ui_Place : UiBase
{
    [SerializeField] private TextMeshProUGUI diceWinnerText;
    [SerializeField] private TextMeshProUGUI turnText;

    public override void HideUi()
    {
        transform.DOScale(0, 0);
    }


    public override void ShowUi()
    {
        transform.DOScale(1, .4f);
        SetActivePlayerText(GameManager.I.activeTeam);
    }
    
    
    public void DiceWinner(TeamColor team)
    {
        diceWinnerText.gameObject.SetActive(true);
        diceWinnerText.text = team == TeamColor.White ? "Player starts" : "Opponent starts";
    }
    
    public override void SetActivePlayerText(TeamColor activeTeam)
    {
        turnText.text = activeTeam == TeamColor.White ? "You" : "Opponent";
    }
}
