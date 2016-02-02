using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class LookEventAction : MonoBehaviour {
    public UnityEvent events;

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
	
	// Update is called once per frame
	void Update () 
    {
        posDelta = transform.position - targetObject.position;

        dotProd = Vector3.Dot(posDelta.normalized, targetObject.forward.normalized);
        dist = Vector3.Distance(transform.position, targetObject.position);

        if (dotProd > dotProdVal && dist < distance)
        {
            events.Invoke();
        }
	}
}
