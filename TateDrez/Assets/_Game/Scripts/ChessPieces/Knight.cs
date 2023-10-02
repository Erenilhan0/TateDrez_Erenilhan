using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Knight : ChessPiece
{
    private readonly Vector2Int[] directions =
    {
        new(1, 2),
        new(1, -2),

        new(-1, -2),
        new(-1, 2),

        new(-2, 1),
        new(2, 1),

        new(2, -1),
        new(-2, -1),
    };


    public override List<BoardPart> AvailableBoardParts()
    {
        if (GameManager.I.currentGamePhase == GamePhase.Place)
        {
            availablePartsInList.Clear();

            for (int i = 0; i < BoardManager.I.allBoardParts.Length; i++)
            {
                var boardPart = BoardManager.I.allBoardParts[i];
                
                if (i != 4 && !boardPart.isFull && !availablePartsInList.Contains(boardPart ))
                {
                    availablePartsInList.Add(boardPart);
                }
            }
        }
        else
        {
            availablePartsInList.Clear();

            for (int i = 0; i < directions.Length; i++)
            {
                for (int j = 0; j < BoardManager.I.allBoardParts.Length; j++)
                {
                    Vector2Int nextCoords = currentBoardPart.coords + directions[i];

                    if (!BoardManager.IsCoordsOnTheBoard(nextCoords)) break;

                    var boardPart = BoardManager.I.GetBoardPartFromPosition(nextCoords);
                    
                    if (boardPart.isFull)
                    {
                        break;
                    }

                    if (!availablePartsInList.Contains(boardPart))
                    {
                        availablePartsInList.Add(boardPart);
                    }
                }
            }
        }

        return availablePartsInList;
    }


   
}