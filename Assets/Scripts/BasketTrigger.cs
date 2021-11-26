using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        if(other.CompareTag(gameObject.tag))
        {
            BasketsController.OnScore?.Invoke(true);
        }
        else
        {
            BasketsController.OnScore?.Invoke(false);

        }
    }
}
