using UnityEngine;
using System.Collections;
using System;

public class MultiSwitch : Device {

    public Switch[] switchesThatMustBeOn;
    public Switch[] switchesThatMustBeOff;

    public Device[] devicesToActivate;
    public Device[] devicesToDeactivate;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public override void Activate()
    {
        bool dontActivateMe = new bool();
        foreach (Switch n in switchesThatMustBeOn)
        {
            if (!n.activated)
            {
                dontActivateMe = true;
            }
        }
        foreach (Switch n in switchesThatMustBeOff)
        {
            if (n.activated)
            {
                dontActivateMe = true;
            }
        }

        if (!dontActivateMe)
        {
            MultiActivate();
        }
    }

    public override void Deactivate()
    {
        Activate();
    }

    void MultiActivate()
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

    void MultiDeactivate()
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

    public void InvertActivation()
    {
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