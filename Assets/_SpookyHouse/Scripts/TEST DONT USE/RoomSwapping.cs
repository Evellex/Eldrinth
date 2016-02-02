using UnityEngine;
using System.Collections;

public class RoomSwapping : MonoBehaviour {
    public Transform targetObject;
    public GameObject[] disableList;
    public GameObject[] enableList;

    public float dotProd;
    public float dist;
    public Vector3 posDelta;
	// Use this for initialization
	void Start () 
    {
        StartRoom();
	}
	
	// Update is called once per frame
	void Update () 
    {
        posDelta = transform.position - targetObject.position;

        dotProd = Vector3.Dot(posDelta.normalized, targetObject.forward.normalized);
        dist = Vector3.Distance(transform.position, targetObject.position);

        if (dotProd > 0.9f && dist < 0.5f)
        {
            SwapRoom();
        }
	}

    void StartRoom()
    {
        foreach (GameObject go in disableList)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in enableList)
        {
            go.SetActive(false);
        }
    }
    void SwapRoom()
    {
        foreach (GameObject go in disableList)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in enableList)
        {
            go.SetActive(true);
        }
    }
}
