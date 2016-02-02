using UnityEngine;
using System.Collections;

public class AnimDevice : Device {
    public Animator animToTrigger;
    public string[] triggerNames;
    public bool delthis;

	void Start () {
        animToTrigger = GetComponentInChildren<Animator>();
	}
	

	void Update () {
	
	}

    public override void Activate()
    {
        activated = true;
        foreach (string t in triggerNames)
        {
            animToTrigger.SetTrigger(t);
        }
        if (delthis)
            Invoke("DelThis", 10);
    }
    public override void Deactivate()
    {
        activated = false;
        foreach (string t in triggerNames)
        {
            animToTrigger.SetTrigger(t);
        }
    }
    public void DelThis()
    {
        Destroy(gameObject);
    }
}