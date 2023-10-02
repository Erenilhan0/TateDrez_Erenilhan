using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum GamePhase
{
    Dice,
    Place,
    Game,
    End
}

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public event Action<GamePhase> OnGamePhaseChange;

    public GamePhase currentGamePhase;
    public bool didPlayerWon;
    public Player player;
    public Opponent opponent;

    public TeamColor activeTeam;
    [SerializeField] private Ui_Game uiGame;
    [SerializeField] private ParticleSystem levelFinishedParticle;
    public bool dynamicMode;
    private int placedPieceCount;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        OnGamePhaseChange += OnOnGamePhaseChange;
    }

    private void OnOnGamePhaseChange(GamePhase obj)
    {
        if (obj == GamePhase.Place)
        {
            placedPieceCount = 0;
        }
    }


    public void ChangeGameState(GamePhase to)
    {
        currentGamePhase = to;
        OnGamePhaseChange?.Invoke(to);
    }

    private void StartLevel()
    {
        didPlayerWon = false;
        ChangeGameState(GamePhase.Dice);
    }

    public void OnClickNextLevel()
    {
        StartLevel();
    }


    public void OnClickRestart()
    {
        StartLevel();
    }


    public void ChangeActiveTeam(TeamColor teamToActive)
    {
        activeTeam = teamToActive;
    }
    


    public void EndTheTurn(TeamColor previousTeam)
    {
        if (currentGamePhase == GamePhase.Place)
        {
            ChangeActiveTeam(previousTeam == TeamColor.White ? TeamColor.Black : TeamColor.White);

            UiManager.I.uis[1].SetActivePlayerText(activeTeam);

            placedPieceCount++;
            if (placedPieceCount >= 6)
            {
                ChangeGameState(GamePhase.Game);
                return;
            }


            if (activeTeam == TeamColor.Black)
            {
                opponent.PlaceChessPiece();
            }
        }
        else
        {
            if (BoardManager.I.IsGameEnded(activeTeam))
            {
                ChangeGameState(GamePhase.End);

                if (didPlayerWon)
                {
                    levelFinishedParticle.Play();
                }
            }
            else
            {
                dynamicMode = !BoardManager.I.IsBoardPlayable();
                uiGame.DynamicMode(dynamicMode);
                
                if (!dynamicMode)
                {
                    ChangeActiveTeam(previousTeam == TeamColor.White ? TeamColor.Black : TeamColor.White);

                }

                UiManager.I.uis[2].SetActivePlayerText(activeTeam);

                if (activeTeam == TeamColor.Black)
                {
                    opponent.OpponentTurn();
                }
               
            }
        }
    }
}