using UnityEngine;
using System.Collections;

public class SetActiveDevice : Device {
	// Use this for initialization
	void Start () 
    {
        gameObject.SetActive(false);
	}
    public override void Activate()
    {
        gameObject.SetActive(true);
    }
    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
