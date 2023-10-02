using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabler : MonoBehaviour
{


    public void DisableThis()
    {
        gameObject.SetActive(false);
    }
}
