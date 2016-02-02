using UnityEngine;
using System.Collections;

public class PlayerLookSwitch : Switch {

    float dotProd;
    float dist;
    Vector3 posDelta;
    [Tooltip("The distance the player must be in to activate")]
    public float distance = 0.5f;
    [Tooltip("The value that the dot product must be larger than to activate")]
    public float dotProdVal = 0.9f;

    public Transform targetObject;

    void Start()
    {
        targetObject = Camera.main.transform;
    }

    void Update()
    {
        posDelta = transform.position - targetObject.position;

        dotProd = Vector3.Dot(posDelta.normalized, targetObject.forward.normalized);
        dist = Vector3.Distance(transform.position, targetObject.position);

        if (dotProd > dotProdVal && dist < distance)
        {
            Activate();
        }
    }

    public override void Activate()
    {
        foreach (Device d in devicesToActivate)
        {
            d.Activate();
        }
    }
    public override void Deactivate()
    {
        foreach (Device d in devicesToActivate)
        {
            d.Deactivate();
        }
    }
    public override void InvertActivation()
    {
        throw new System.NotImplementedException();
    }
    
}
