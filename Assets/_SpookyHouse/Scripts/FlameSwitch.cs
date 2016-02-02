using UnityEngine;
using System.Collections;
using System;

public class FlameSwitch : Switch {
    Flamable flame;

    void Start()
    {
        flame = GetComponentInChildren<Flamable>();
        if (flame == null)
        {
            Debug.Log("WARNING! THE FLAME COMPONENT IS NULL ON " + gameObject.name);
        }
    }

    void Update()
    {
        if (flame.onFire && !activated)
        {
            activated = true;
            Activate();
        }
        else if (!flame.onFire && activated)
        {
            activated = false;
            Deactivate();
        }
    }

	public override void Activate()
    {
        foreach(Device d in devicesToActivate)
        {
            d.Activate();
        }
        foreach (Device d in devicesToDeactivate)
        {
            d.Deactivate();
        }
        activated = true;
    }

    public override void  Deactivate()
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
        foreach (Device d in devicesToActivate)
        {
            if (d.activated)
            {
                d.Deactivate();
            } else
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
