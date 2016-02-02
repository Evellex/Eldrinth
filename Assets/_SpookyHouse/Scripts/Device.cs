    using UnityEngine;
using System.Collections;

public abstract class Device : MonoBehaviour {

    public bool activated;

    public abstract void Activate();
    public abstract void Deactivate();
}
