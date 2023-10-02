using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlaceStateController : MonoBehaviour
{
    [SerializeField] private Transform playerGround;
    [SerializeField] private Transform opponentGround;

    [SerializeField] private ChessPiece[] chessPieces;

    private void Start()
    {
        GameManager.I.OnGamePhaseChange += OnOnGamePhaseChange;
        SetUpPieces();
    }

    private void OnOnGamePhaseChange(GamePhase obj)
    {
        switch (obj)
        {
            case GamePhase.Dice:
                SetUpPieces();
                break;
            case GamePhase.Place:
                StartGroundAnim(true);
                break;
            
            case GamePhase.Game:
                StartGroundAnim(false);
                break;
        
            case GamePhase.End:
                break;
        }
    }

    private void StartGroundAnim(bool open)
    {
        if (open)
        {
            playerGround.DOMoveZ(0.5f, .5f);
            opponentGround.DOMoveZ(5f, .5f);

        }
        else
        {
            playerGround.DOMoveZ(-2, 1);
            opponentGround.DOMoveZ(10, 1);

        }
    }

    private void SetUpPieces()
    {
        for (int i = 0; i < chessPieces.Length; i++)
        {
            var chessPiece = chessPieces[i];

            Transform transform1;
            (transform1 = chessPiece.transform).SetParent(chessPiece.teamColor == TeamColor.White ? playerGround : opponentGround);

            var xPos = i % 3;
            
            transform1.localPosition = new Vector3(xPos, 0, -1.75f);
            chessPiece.piecePosition = transform1.localPosition;
            chessPiece.isPlaced = false;
        }
    }

}
