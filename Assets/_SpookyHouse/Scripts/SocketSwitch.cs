using UnityEngine;
using System.Collections;

public class SocketSwitch : Switch {
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
        //throw new System.NotImplementedException();
    }

}
