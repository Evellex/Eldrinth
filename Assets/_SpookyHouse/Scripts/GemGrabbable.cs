using UnityEngine;
using System.Collections;

public class GemGrabbable : GrabbableObj 
{

    SocketSwitch socketSwitch;
    bool held = false;
    public override void OnGrab()
    {
        rb.isKinematic = false;
        if (joint)
            DestroyImmediate(joint);
        held = true;
        if(socketSwitch)
            RemoveFromSocket();
    }
    public override void OnDrop()
    {
        held = false;
        rb.isKinematic = false;

        if (socketSwitch)
            SocketIntoSocket();
        //base.OnDrop();
    }

    void OnTriggerEnter(Collider collider)
    {
        SocketSwitch ss = collider.GetComponent<SocketSwitch>();
        if (ss)
            socketSwitch = ss;

        if (held == false)
        {
            SocketIntoSocket();
        }
    }
    void OnTriggerExit(Collider collider)
    {
        SocketSwitch ss = collider.GetComponent<SocketSwitch>();
        if (socketSwitch == ss)
            socketSwitch = null;
    }

    void SocketIntoSocket()
    {
        if (socketSwitch == false)
            return;

        transform.position = socketSwitch.transform.position;
        transform.rotation = socketSwitch.transform.rotation;

        rb.isKinematic = true;

        socketSwitch.Activate();
    }
    void RemoveFromSocket()
    {
        socketSwitch.Deactivate();
    }
}
