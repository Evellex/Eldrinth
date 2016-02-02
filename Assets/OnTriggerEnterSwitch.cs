using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider))]

public class OnTriggerEnterSwitch : Switch {
    public bool playerHeadOnly;
    Collider colToTrigger;
	// Use this for initialization
	void Start () {
        colToTrigger = GetComponent<Collider>();
        if (!colToTrigger.isTrigger)
        {
            Debug.Log("WARNING! THE COLLIDER ATTACHED TO " + gameObject.name+ "MUST BE A TRIGGER,IT HAS BEEN SET TO TRIGGER FOR NOW");
            colToTrigger.isTrigger = true;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (playerHeadOnly && other.gameObject.GetComponent<PlayerHead>() != null)
        {
            Activate();
        } else if (!playerHeadOnly)
        {
            Activate();
        }
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