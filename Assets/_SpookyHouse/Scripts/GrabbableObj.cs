using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public class GrabbableObj : MonoBehaviour {

    public Rigidbody rb;
    [Tooltip("Should this object snap to the hand?")]
    public bool shouldSnap;
    protected FixedJoint joint;

    List<Collider> hardColliders;

    Vector3 startpos;
    Quaternion startRot;
    bool startKenematic;

    bool inEmergency = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<FixedJoint>();
        if (inEmergency)
        {
            hardColliders = new List<Collider>();
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                if (col.isTrigger == false)
                {
                    hardColliders.Add(col);
                }
            }
        }
        startpos = transform.position;
        startRot = transform.rotation;
        startKenematic = rb.isKinematic;

    }
    virtual public void OnGrab()
    {
        //this was buggy as fuck, will try again later 
        //transform.parent = null;
        rb.isKinematic = false;
        if (joint)
            DestroyImmediate(joint);

        Flamable fmb = GetComponentInChildren<Flamable>();
        if (fmb)
            fmb.fireAnim.SetBool("playerTorch", true);

        if (inEmergency)
        {
            foreach (Collider col in hardColliders)
            {
                col.enabled = false;
            }
        }
    }
    virtual public void OnDrop()
    {
        Flamable fmb = GetComponentInChildren<Flamable>();
        if (fmb)
            fmb.fireAnim.SetBool("playerTorch", true);
        if (inEmergency)
        {
            foreach (Collider col in hardColliders)
            {
                col.enabled = true;
            }
        }
    }
    /*void OnCollisionEnter(Collision collision)
    {
        transform.parent = collision.transform;
    }*/
   
}
