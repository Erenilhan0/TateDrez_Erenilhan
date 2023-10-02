using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoBehaviour
{
    public static BoardManager I;
    public BoardPart[] allBoardParts;
    public int boardSize;

    private readonly Vector3Int[] winningConditions =
    {
        new(0, 1, 2),
        new(3, 4, 5),
        new(6, 7, 8),
        new(0, 3, 6),
        new(1, 4, 7),
        new(2, 5, 8),
        new(0, 4, 8),
        new(2, 4, 6),
    };

    private List<BoardPart> _completedGrounds;

    [SerializeField] private Player player;
    [SerializeField] private Opponent opponent;
    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        GameManager.I.OnGamePhaseChange += OnOnGamePhaseChange;
    }

    private void OnOnGamePhaseChange(GamePhase obj)
    {
        if (obj == GamePhase.Dice)
        {
            ResetBoard();
        }
    }

    private static Vector3 GetPositionFromCoords(Vector2Int coords, float yPos)
    {
        return new Vector3(coords.x, yPos, coords.y);
    }

    public static bool IsCoordsOnTheBoard(Vector2Int coords)
    {
        return coords.x is <= 2 and >= 0 && coords.y is <= 2 and >= 0;
    }

    public BoardPart GetBoardPartFromPosition(Vector2Int coords)
    {
        foreach (var part in allBoardParts)
        {
            if (part.transform.position == GetPositionFromCoords(coords, part.transform.position.y))
            {
                return part;
            }
        }

        return null;
    }

    public bool IsBoardPlayable()
    {
        opponent.CalculateAllPossibleMoves();
        player.CalculateAllMoves();
        if (player.availableMoves.Count == 0 && opponent.availableMoves.Count == 0)
        {
            GameManager.I.ChangeGameState(GamePhase.Dice);
            return false;
        }

        return true;
    }

    public bool CanWin(BoardPart currentBoard, BoardPart nextBoard, TeamColor teamColor)
    {
        foreach (var condition in winningConditions)
        {
            var xBoard = allBoardParts[condition.x];
            var yBoard = allBoardParts[condition.y];
            var zBoard = allBoardParts[condition.z];


            var win = ((currentBoard != yBoard && currentBoard != zBoard && nextBoard == xBoard) &&
                       yBoard.isFull && zBoard.isFull && !nextBoard.isFull &&
                       (xBoard.teamOnPart == teamColor &&
                        yBoard.teamOnPart == teamColor));
            if (win)
            {
                return true;
            }

            foreach (var t1 in winningConditions)
            {
                win = ((currentBoard != xBoard && currentBoard != zBoard && nextBoard == yBoard) &&
                       xBoard.isFull && zBoard.isFull && !nextBoard.isFull &&
                       (xBoard.teamOnPart == teamColor &&
                        zBoard.teamOnPart == teamColor));
                if (win)
                {
                    return true;
                }

                for (int z = 0; z < winningConditions.Length; z++)
                {
                    win = ((currentBoard != xBoard && currentBoard != yBoard && nextBoard == zBoard) &&
                           xBoard.isFull && yBoard.isFull && !nextBoard.isFull &&
                           (xBoard.teamOnPart == teamColor &&
                            yBoard.teamOnPart == teamColor));
                    if (win)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool IsGameEnded(TeamColor teamColor)
    {
        _completedGrounds.Clear();
        for (int i = 0; i < winningConditions.Length; i++)
        {
            var firstBoardPart = allBoardParts[winningConditions[i].x];
            var secondBoardPart = allBoardParts[winningConditions[i].y];
            var thirdBoardPart = allBoardParts[winningConditions[i].z];

            var gameFinished = (firstBoardPart.isFull && secondBoardPart.isFull && thirdBoardPart.isFull &&
                                (firstBoardPart.teamOnPart == teamColor &&
                                 secondBoardPart.teamOnPart == teamColor &&
                                 thirdBoardPart.teamOnPart == teamColor));


            if (gameFinished)
            {
                GameManager.I.didPlayerWon = firstBoardPart.teamOnPart == TeamColor.White;
                _completedGrounds.Add(firstBoardPart);
                _completedGrounds.Add(secondBoardPart);
                _completedGrounds.Add(thirdBoardPart);
                StartCoroutine(LevelEnded());
                return true;
            }
        }

        return false;
    }


    private IEnumerator LevelEnded()
    {
        foreach (var completedGround in _completedGrounds)
        {
            completedGround.LevelFinishAnim(true);
            yield return new WaitForSeconds(.25f);
        }
        GameManager.I.ChangeGameState(GamePhase.End);
    }

    private void ResetBoard()
    {
        foreach (var boardPart in allBoardParts)
        {
            boardPart.isFull = false;
            boardPart.teamOnPart = TeamColor.None;
            boardPart.LevelFinishAnim(false);

        }
    }
}