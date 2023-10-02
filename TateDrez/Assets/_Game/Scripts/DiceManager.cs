using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager I;
    [SerializeField] private Dice playerDice;
    [SerializeField] private Dice opponentDice;
    [SerializeField] private Ui_Place uiPlace;
    [SerializeField] private Ui_Dice uiDice;
    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        GameManager.I.OnGamePhaseChange += OnOnGamePhaseChange;
        SetUpDices();
    }

    private void OnOnGamePhaseChange(GamePhase obj)
    {
        switch (obj)
        {
            case GamePhase.Dice:
                SetUpDices();
                break;
            case GamePhase.Place:
                OpenCloseDices(false);
                break;
        }
    }

    private void SetUpDices()
    {
        OpenCloseDices(true);

        playerDice.SetupThis();
        opponentDice.SetupThis();
    }

    private void OpenCloseDices(bool open)
    {
        if (open)
        {
            playerDice.gameObject.SetActive(true);
            opponentDice.gameObject.SetActive(true);
        }
        else
        {
            playerDice.gameObject.SetActive(false);
            opponentDice.gameObject.SetActive(false);
        }
      
    }
    
    public void OpponentTurn()
    {
        StartCoroutine(opponentDice.RollOpponentDice());
    }
    
    public void WhoWillStart()
    {
        if (TeamToStart() == TeamColor.None)
        {
            RestartDicePhase();
            return;
        }

        GameManager.I.ChangeActiveTeam(TeamToStart());
        
        GameManager.I.ChangeGameState(GamePhase.Place);
        uiPlace.DiceWinner(GameManager.I.activeTeam);

    }

    private TeamColor TeamToStart()
    {
        if (playerDice.diceResult == opponentDice.diceResult)
        {
            return TeamColor.None;
        }

        if (playerDice.diceResult > opponentDice.diceResult )
        {
            return TeamColor.White;
        }
        
        return TeamColor.Black;
    }


    private void RestartDicePhase()
    {
        SetUpDices();
        uiDice.rollButtonGo.SetActive(true);
    }

    
}
