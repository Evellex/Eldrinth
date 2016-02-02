using UnityEngine;
using System.Collections;

public class Grabbers : MonoBehaviour 
{
    SteamVR_Controller.Device device;

    public Rigidbody attachPoint;

    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;

    GrabbableObj objNearHand;
    Flamable fireNearHand;

    Light light;

    // Use this for initialization
	void Start () 
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObj.index);
        light = GetComponentInChildren<Light>();
	}
    //should be called OnUpdate()
    void Update()
    {
        if (objNearHand != null && joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (objNearHand.shouldSnap)
            {
                objNearHand.rb.position = attachPoint.position;
                objNearHand.rb.rotation = attachPoint.rotation;
            }
            //DebugConsole.dc.AddLine("Should have grabbed");
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = objNearHand.rb;
            objNearHand.OnGrab();

            light.enabled = false;
        }

        if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            joint.connectedBody.isKinematic = false;
            joint.connectedBody.GetComponent<GrabbableObj>().OnDrop();

            joint.connectedBody.velocity = device.velocity;
            joint.connectedBody.angularVelocity = device.angularVelocity;

            joint.connectedBody = null;
            Object.DestroyImmediate(joint);

            light.enabled = true;
        }
        if (fireNearHand != null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            fireNearHand.Extinguish();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //DebugConsole.dc.AddLine("OnTriggerEnterCalled");
        GrabbableObj obj = collider.attachedRigidbody.gameObject.GetComponent<GrabbableObj>();
        if (obj != null && joint == null)// && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            objNearHand = obj;
            /*if (obj.shouldSnap)
            {
                obj.rb.position = attachPoint.position;
                obj.rb.rotation = attachPoint.rotation;
            }
            //DebugConsole.dc.AddLine("Should have grabbed");
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = obj.rb;
            obj.OnGrab();*/
        }
        Flamable fmb = collider.gameObject.GetComponent<Flamable>();
        if (fmb != null)
        {
            fireNearHand = fmb;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        GrabbableObj obj = collider.attachedRigidbody.gameObject.GetComponent<GrabbableObj>();
        if (obj == objNearHand)
            objNearHand = null;

        Flamable fmb = collider.gameObject.GetComponent<Flamable>();
        if (fmb == fireNearHand)
        {
            fireNearHand = null;
        }
    }
}
