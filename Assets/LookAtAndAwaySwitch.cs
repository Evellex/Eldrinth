using UnityEngine;
using System.Collections;
using System;

public class LookAtAndAwaySwitch : Switch {

    bool lookedAt;

    float dotProd;
    float dist;
    Vector3 posDelta;
    [Tooltip("The distance the player must be in to activate")]
    public float distance = 1.5f;
    [Tooltip("The value that the dot product must be larger than to tag as looked at")]
    public float dotProdValLookAt = 0.9f;
    [Tooltip("The value that the dot product must be smaller than to activate")]
    public float dotProdValLookAway = -0.1f;

    public Transform targetObject;

    void Start () {
        targetObject = Camera.main.transform;
    }
	
	void Update () {
        posDelta = transform.position - targetObject.position;

        dotProd = Vector3.Dot(posDelta.normalized, targetObject.forward.normalized);
        dist = Vector3.Distance(transform.position, targetObject.position);
        if (!lookedAt && !activated)
        {
            if (dotProd > dotProdValLookAt && dist < distance)
            {
                lookedAt = true;
            }
        } else if (dotProd < dotProdValLookAway && dist < distance)
        {
            Activate();
        } if (lookedAt && activated && dotProd > dotProdValLookAt && dist < distance)
        {
            Deactivate();
        }
    }

    public override void Activate()
    {
        foreach (Device d in devicesToActivate)
        {
            d.Activate();
        }
        foreach (Device d in devicesToDeactivate)
        {
            d.Deactivate();
        }
        activated = true;
    }

    public override void Deactivate()
    {
        foreach (Device d in devicesToActivate)
        {
            d.Deactivate();
        }
        foreach (Device d in devicesToDeactivate)
        {
            d.Activate();
        }
        activated = false;
    }

    public override void InvertActivation()
    {

    }
}