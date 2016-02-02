using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	// Use this for initialization
    public float force;
    Rigidbody rb;
    public Vector3 rbVel;
    AudioSource boltHitSound;
    Collider boltCol;
    bool flying = true;
	void Start () 
    {
        rb = GetComponent<Rigidbody>();
        boltCol = GetComponentInChildren<Collider>();
        boltHitSound = GetComponent<AudioSource>();
        rb.velocity = transform.forward * force;
	}

    void OnTriggerEnter(Collider collider)
    {
        if (flying)
        {
            Flamable fmb = collider.GetComponent<Flamable>();
            if (fmb != null)
            {
                fmb.Extinguish();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (flying && collision.gameObject.GetComponent<BoltSwitch>()!= null)
        {
            collision.gameObject.GetComponent<BoltSwitch>().Activate();
        }
        if (flying)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            boltHitSound.Play();
            Destroy(boltCol);
            transform.position = transform.position + (transform.forward / 20);
            transform.parent = collision.gameObject.transform;
        }
        else
        {
            rb.useGravity = true;
        }
        flying = false;
    }

}
