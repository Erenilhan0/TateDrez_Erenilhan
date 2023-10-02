using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    [SerializeField] private TeamColor team;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask hitLm;
    private Vector3 startPosition;

    [Header("Force Values")]
    [SerializeField] private float rollForce;
    [SerializeField] private float torqueForce;
    
    [HideInInspector] public int diceResult;

    private void Awake()
    {
        startPosition = transform.position;
    }
    

    public void SetupThis()
    {
        rb.isKinematic = true;
        transform.position = startPosition;
        transform.localEulerAngles =
            new Vector3(Random.Range(-180f, 180f), Random.Range(-180f,180f), Random.Range(-180f, 180f));
    }

    public IEnumerator RollOpponentDice()
    {
        yield return new WaitForSeconds(Random.Range(.5f,1f));
        RollTheDice();
    }

    public void RollTheDice()
    {
        var randomX = Random.Range(0, 2f);

        var direction = new Vector3(randomX, 1.5f, 1) - transform.position;
        direction.Normalize();
        rb.isKinematic = false;

        rb.AddForce(direction * rollForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);

        StartCoroutine(IsRollStopped());
    }

    private IEnumerator IsRollStopped()
    {
        while (!rb.IsSleeping())
        {
            yield return null;
        }

        yield return null;

        diceResult = GetDiceResult();
        if (team == TeamColor.White)
        {
            DiceManager.I.OpponentTurn();
        }
        else
        {
            DiceManager.I.WhoWillStart();
        }
    }
    
    

    private int GetDiceResult()
    {
        var transform1 = transform;
        var forward = transform1.forward;
        var right = transform1.right;
        var up = transform1.up;

        Vector3[] diceDirections =
        {
            -forward,   //1
            right,      //2   
            -up,        //3    
            up,        //4  
            -right,     //5
            forward,    //6
        };

        for (int i = 0; i < diceDirections.Length; i++)
        {
            if (Physics.Raycast(transform.position, diceDirections[i], .5f, hitLm))
            {
                return i + 1;
            }
        }

        return 0;
    }
}