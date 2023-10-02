using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardPart : MonoBehaviour
{

    public Vector2Int coords;
    public TeamColor teamOnPart;
    public bool isFull;
    
    [SerializeField] private GameObject availableIndicator;
    
    [SerializeField] private MeshRenderer meshRenderer;
    
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material levelFinishGroundMaterial;


    private void Start()
    {
        var position = transform.position;
        coords = new Vector2Int((int)position.x, (int)position.z);
    }

    public void BoardIsAvailable(bool isAvailable)
    {
        availableIndicator.SetActive(isAvailable);
    }

    public void HighlightBoard(bool highlight)
    {
        meshRenderer.material = highlight ? highlightMaterial : defaultMaterial;
    }

    public void LevelFinishAnim(bool finished)
    {
        meshRenderer.material = finished ? levelFinishGroundMaterial : defaultMaterial;
    }
    
}