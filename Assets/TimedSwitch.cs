using UnityEngine;
using System.Collections;
using System;

public class TimedSwitch : Device {

    public GameObject s1, s2;
    public float time;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        Invoke("SwapS", time);
    }

    public override void Deactivate()
    {
        
    }

    void SwapS()
    {
        s1.SetActive(false);
        s2.SetActive(true);
    }
}
