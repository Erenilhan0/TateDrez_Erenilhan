using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{

    public static GlobalGameManager I;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);

        }
    }
}
