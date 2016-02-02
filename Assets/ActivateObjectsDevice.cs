using UnityEngine;
using System.Collections;
using System;

public class ActivateObjectsDevice : Device {

    public GameObject[] gameObjectsToSetActive;

	void Start () {
	
	}

    public override void Activate()
    {
        foreach (GameObject g in gameObjectsToSetActive)
        {
            g.SetActive(true);
        }
    }
    public override void Deactivate()
    {
        foreach (GameObject g in gameObjectsToSetActive)
        {
            g.SetActive(false);
        }
    }
}