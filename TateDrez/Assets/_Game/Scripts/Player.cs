using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Plugins;
using UnityEngine;
using Random = UnityEngine.Random;


public class Player : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform pieceParent;

    //Dragging
    [SerializeField] private LayerMask chessPieceLm;
    [SerializeField] private LayerMask boardPieceLm;

    private ChessPiece _selectedChessPiece;
    private bool _isDraggingObject;
    private Vector3 _targetPosition;
    private BoardPart _currentBoardPart;


    public TeamColor teamColor;
    public ChessPiece[] chessPieces;
    public List<AvailableMove> availableMoves;


    private void Start()
    {
        GameManager.I.OnGamePhaseChange += OnOnGamePhaseChange;
    }

    private void OnOnGamePhaseChange(GamePhase obj)
    {
        if (obj == GamePhase.Game)
        {
            PlayerTurn();
        }
    }

    private void Update()
    {
        if (GameManager.I.currentGamePhase != GamePhase.Game &&
            GameManager.I.currentGamePhase != GamePhase.Place) return;

        if (GameManager.I.activeTeam != TeamColor.White) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out var pieceHit, 20, chessPieceLm))
            {
                pieceHit.transform.TryGetComponent(out _selectedChessPiece);
                
                if (GameManager.I.currentGamePhase == GamePhase.Place && _selectedChessPiece.isPlaced) return;
                PieceSelected();
                
                _isDraggingObject = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!_isDraggingObject) return;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out var groundHit, 20, boardPieceLm))
            {
                if (_currentBoardPart != null)
                {
                    if (_currentBoardPart.gameObject != groundHit.transform.gameObject)
                    {
                        _currentBoardPart.HighlightBoard(false);
                    }
                }

                if (groundHit.transform.TryGetComponent(out _currentBoardPart))
                {
                    _currentBoardPart.HighlightBoard(true);
                }

                _targetPosition = groundHit.point;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!_isDraggingObject) return;
            _isDraggingObject = false;

            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out var groundHit, 20, boardPieceLm))
            {
                if (groundHit.transform.TryGetComponent(out _currentBoardPart))
                {
                    PlaceChessPiece(_currentBoardPart);
                }
                else
                {
                    if (GameManager.I.currentGamePhase == GamePhase.Place)
                    {
                        ReturnSelectGround();
                    }
                    else
                    {
                        PlaceChessPiece(_selectedChessPiece.currentBoardPart);
                    }
                }
            }
            else
            {
                if (GameManager.I.currentGamePhase == GamePhase.Place)
                {
                    ReturnSelectGround();
                }
                else
                {
                    PlaceChessPiece(_selectedChessPiece.currentBoardPart);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isDraggingObject) return;

        _selectedChessPiece.transform.position =
            Vector3.Lerp(_selectedChessPiece.transform.position, _targetPosition, 0.55f);
    }

    public void PlayerTurn()
    {
        CalculateAllMoves();
        if (availableMoves.Count() == 0)
        {
            GameManager.I.EndTheTurn(teamColor);
        }
    }
    
    
    public void CalculateAllMoves()
    {
        availableMoves.Clear();

        foreach (var chessPiece in chessPieces)
        {
            var availableBoardParts = chessPiece.AvailableBoardParts();

            foreach (var availablePart in availableBoardParts)
            {
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
    }

    private void PieceSelected()
    {
        _currentBoardPart = _selectedChessPiece.currentBoardPart;

        var availableBoards = _selectedChessPiece.AvailableBoardParts();

        foreach (var t in availableBoards)
        {
            t.BoardIsAvailable(true);
        }
    }

    private void ReturnSelectGround()
    {
        _selectedChessPiece.transform.DOLocalMove(_selectedChessPiece.piecePosition, 0.25f);

        foreach (var boardPart in BoardManager.I.allBoardParts)
        {
            boardPart.HighlightBoard(false);
            boardPart.BoardIsAvailable(false);
        }
    }

    private void PlaceChessPiece(BoardPart targetBoardPart)
    {
        if (GameManager.I.currentGamePhase == GamePhase.Place)
        {
            if (_currentBoardPart.isFull || !_selectedChessPiece.AvailableBoardParts().Contains(targetBoardPart))
            {
                ReturnSelectGround();
                _currentBoardPart.HighlightBoard(false);
                return;
            }

            _selectedChessPiece.transform.SetParent(pieceParent);
            _selectedChessPiece.isPlaced = true;
        }
        
        _selectedChessPiece.MoveTo(targetBoardPart, 0.25f, teamColor);

       
        foreach (var boardPart in BoardManager.I.allBoardParts)
        {
            boardPart.HighlightBoard(false);
            boardPart.BoardIsAvailable(false);
        }
    }
}