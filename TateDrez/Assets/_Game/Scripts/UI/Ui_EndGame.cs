
using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_EndGame : UiBase
{
    

    [SerializeField] private GameObject winPanel, losePanel;
    
    
    public override void HideUi()
    {
        transform.DOScale(0, 0);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }


    public override void ShowUi()
    {
        transform.DOScale(1, 0);
        StartCoroutine(OpenPanel());
    }

    private IEnumerator OpenPanel()
    {
        yield return new WaitForSeconds(1);
        if (GameManager.I.didPlayerWon)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }

    }
    
    
    
    
    
    
}
