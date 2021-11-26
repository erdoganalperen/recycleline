using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingLine : MonoBehaviour
{
    public float speed = 1f;
    private void OnCollisionStay(Collision other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
}