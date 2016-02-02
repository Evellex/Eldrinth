using UnityEngine;
using System.Collections;

public class DoorOpenDevice : Device 
{
    IEnumerator openRoutine;
    public Vector3 openOffset;
    Vector3 initPos;

    void Start()
    {
        initPos = transform.localPosition;
    }

    public override void Activate()
    {
        activated = true;
        transform.localPosition = initPos + openOffset;
        //throw new System.NotImplementedException();
    }
    public override void Deactivate()
    {
        activated = false;
        transform.localPosition = initPos;

        //throw new System.NotImplementedException();
    }

}
