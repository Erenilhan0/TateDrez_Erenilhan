using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bishop : ChessPiece
{
    private readonly Vector2Int[] directions =
    {
        new(1, 1),
        new(1, -1),
        new(-1, 1),
        new(-1, -1),
    };

    public override List<BoardPart> AvailableBoardParts()
    {
        if (GameManager.I.currentGamePhase == GamePhase.Place)
        {
            availablePartsInList.Clear();

            for (int i = 0; i < BoardManager.I.allBoardParts.Length; i++)
            {
                var boardPart = BoardManager.I.allBoardParts[i];

                if (!boardPart.isFull &&!availablePartsInList.Contains(boardPart)) 
                {
                    availablePartsInList.Add(boardPart);
                }
            }
           
        }
        else
        {
            availablePartsInList.Clear();


            foreach (var t in directions)
            {
                var directionMultiplier = 0;

                while (directionMultiplier <= BoardManager.I.boardSize)
                {
                    directionMultiplier++;

                    var targetCoords = currentBoardPart.coords + t * directionMultiplier;

                    if (!BoardManager.IsCoordsOnTheBoard(targetCoords)) break;

                    var boardPart = BoardManager.I.GetBoardPartFromPosition(targetCoords);

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