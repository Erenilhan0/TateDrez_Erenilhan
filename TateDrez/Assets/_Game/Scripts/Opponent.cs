using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class AvailableMove
{
    public ChessPiece pieceToMove;
    public BoardPart boardPart;
}

public class Opponent : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform pieceParent;
    [SerializeField] private ChessPiece[] chessPieces;
    [SerializeField] private TeamColor teamColor;

    private int _placedPieceCount;
    private BoardPart _targetBoardPart;
    
    public List<AvailableMove> availableMoves;
    private AvailableMove _currentMove;
    
    private void Start()
    {
        GameManager.I.OnGamePhaseChange += OnOnGamePhaseChange;
    }
    

    private void OnOnGamePhaseChange(GamePhase obj)
    {
        switch (obj)
        {
            case GamePhase.Dice:
                break;
            case GamePhase.Place:
            {
                _placedPieceCount = 0;
                if (GameManager.I.activeTeam == TeamColor.Black)
                {
                    PlaceChessPiece();
                }

                break;
            }
            case GamePhase.Game:
            {
                if (GameManager.I.activeTeam == TeamColor.Black)
                {
                    SetMove();
                }

                break;
            }
        }
    }
    
    
    public void OpponentTurn()
    {
        SetMove();
    }

    public void PlaceChessPiece()
    {
        availableMoves.Clear();

        CalculateAllPossibleMovesForPiece(chessPieces[_placedPieceCount]);

        var random = Random.Range(0, availableMoves.Count);
        var randomMove = availableMoves[random];

        StartCoroutine(MakeMove(randomMove));

        _placedPieceCount++;
    }


   

    private void SetMove()
    {
        CalculateAllPossibleMoves();
        
        if (availableMoves.Count == 0)
        {
            GameManager.I.OpenDynamicMode(true, teamColor);
        }
        else
        {
            StartCoroutine(MakeMove(ChooseMove()));
        }

    }
    
    private void CalculateAllPossibleMovesForPiece(ChessPiece chessPiece)
    {
        var availableParts = chessPiece.AvailableBoardParts();

        for (int i = 0; i < availableParts.Count; i++)
        {
            var availablePart = chessPiece.AvailableBoardParts()[i];

            var availableMove = new AvailableMove
            {
                pieceToMove = chessPiece,
                boardPart = availablePart
            };
            
            if (!availableMoves.Contains(availableMove))
            {
                availableMoves.Add(availableMove);
            }
        }
    }

    public void CalculateAllPossibleMoves()
    {
        availableMoves.Clear();
        
        foreach (var chessPiece in chessPieces)
        {
            CalculateAllPossibleMovesForPiece(chessPiece);
        }
    }

    private AvailableMove ChooseMove()
    {
        if (CanOpponentWin())
        {
            return _currentMove;
        }

        if (CanPlayerWin())
        {
            return _currentMove;
        }


        var random = Random.Range(0, availableMoves.Count);
        var randomMove = availableMoves[random];
        return randomMove;
    }


    private bool CanOpponentWin()
    {
        foreach (var chessPiece in chessPieces)
        {
            var availableBoards = chessPiece.AvailableBoardParts();
            for (int j = 0; j < availableBoards.Count; j++)
            {
                var nextBoardPart = availableBoards[j];

                if (BoardManager.I.CanWin(chessPiece.currentBoardPart, nextBoardPart, TeamColor.Black))
                {
                    _currentMove = GetMoveFromPiece(chessPiece, nextBoardPart);
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanPlayerWin()
    {
        foreach (var playerPiece in player.chessPieces)
        {
            for (int j = 0; j < playerPiece.AvailableBoardParts().Count; j++)
            {
                var nextBoardPart = playerPiece.AvailableBoardParts()[j];

                if (BoardManager.I.CanWin(playerPiece.currentBoardPart, nextBoardPart, TeamColor.White))
                {
                    for (int k = 0; k < availableMoves.Count; k++)
                    {
                        if (availableMoves[k].boardPart == nextBoardPart)
                        {
                            _currentMove = availableMoves[k];
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private AvailableMove GetMoveFromPiece(ChessPiece chessPiece, BoardPart boardPart)
    {
        foreach (var move in availableMoves)
        {
            if (move.pieceToMove == chessPiece && move.boardPart == boardPart)
            {
                return move;
            }
        }

        return null;
    }

    private IEnumerator MakeMove(AvailableMove move)
    {
        
        var randomTime = Random.Range(1f, 1.25f);
        yield return new WaitForSeconds(randomTime);

        var chessPiece = move.pieceToMove;
        var boardPart = move.boardPart;
        chessPiece.piecePosition = boardPart.transform.position;

        chessPiece.MoveTo(boardPart, .6f, teamColor);
        if (GameManager.I.currentGamePhase == GamePhase.Place)
        {
            chessPiece.transform.SetParent(pieceParent);
        }
    }
}