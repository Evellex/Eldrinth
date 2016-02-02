using UnityEngine;
using System.Collections;
using System;

public class BoltSwitch : Switch {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        activated = true;
        foreach (Device d in devicesToActivate)
        {
            d.Activate();
        }
        foreach (Device d in devicesToDeactivate)
        {
            d.Deactivate();
        }
    }

    public override void Deactivate()
    {
        activated = false;
        foreach (Device d in devicesToActivate)
        {
            d.Deactivate();
        }
        foreach (Device d in devicesToDeactivate)
        {
            d.Activate();
        }
    }

    public override void InvertActivation()
    {
        activated = !activated;
        foreach (Device d in devicesToActivate)
        {
            if (d.activated)
            {
                d.Deactivate();
            }
            else
            {
                d.Activate();
            }
        }
        foreach (Device d in devicesToDeactivate)
        {
            if (d.activated)
            {
                d.Deactivate();
            }
            else
            {
                d.Activate();
            }
        }
    }
}