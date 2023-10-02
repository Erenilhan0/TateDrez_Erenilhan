using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Ui_Dice : UiBase
{
    public GameObject rollButtonGo;
    public override void HideUi()
    {
        transform.DOScale(0, 0);
    }


    public override void ShowUi()
    {
        transform.DOScale(1, 0);
        rollButtonGo.SetActive(true);
    }

   
    
    
    
    
}
