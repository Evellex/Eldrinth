using UnityEngine;
using System.Collections;

public abstract class Switch : MonoBehaviour {
    public bool invertActivation;
    public bool activated;

    public Device[] devicesToActivate;
    public Device[] devicesToDeactivate;

    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void InvertActivation();
}
