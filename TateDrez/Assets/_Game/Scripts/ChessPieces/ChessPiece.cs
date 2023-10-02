using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public enum TeamColor
{
    White,
    Black,
    None
}

public abstract class ChessPiece : MonoBehaviour
{
    public TeamColor teamColor;
    public Vector3 piecePosition;
    public BoardPart currentBoardPart;
    public List<BoardPart> availablePartsInList;
    public ParticleSystem landingParticle;
    public bool isPlaced;
    
    public abstract List<BoardPart> AvailableBoardParts();

    
    public void MoveTo(BoardPart targetBoardPart, float moveSpeed, TeamColor pieceColor)
    {
        if (IsPiecePlaceable(targetBoardPart))
        {
            if (GameManager.I.currentGamePhase == GamePhase.Game)
            {
                currentBoardPart.isFull = false;
                currentBoardPart.teamOnPart = TeamColor.None;
            }


            currentBoardPart = targetBoardPart;
            currentBoardPart.isFull = true;
            currentBoardPart.teamOnPart = pieceColor;
            piecePosition = targetBoardPart.transform.position;


            transform.DOMove(piecePosition, moveSpeed).OnComplete((() =>
            {
                landingParticle.Play();
                GameManager.I.EndTheTurn(pieceColor);
            }));
        }
        else
        {
            transform.DOMove(piecePosition, moveSpeed);
        }
    }

    private bool IsPiecePlaceable(BoardPart boardPartToPlace)
    {
        return availablePartsInList.Contains(boardPartToPlace) && !boardPartToPlace.isFull;
    }
}