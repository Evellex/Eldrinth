using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SphereCollider))]

public class Flamable : MonoBehaviour {
    public bool onFire, inextinguishable;
    AudioSource flameSound;
    SphereCollider flameCollider;
    public Animator fireAnim;
	// Use this for initialization
	void Start () {
        flameSound = GetComponent<AudioSource>();
        fireAnim = GetComponentInParent<Animator>();
        flameCollider = GetComponent<SphereCollider>();
        if (onFire)
        {
            if (flameSound != null)
                flameSound.Play();
            Ignite();
        } else
        {
            Extinguish();
        }
        if (!flameCollider.isTrigger)
        {
            flameCollider.isTrigger = true;
            Debug.Log("WARNING! The collider on a flame has been set to trigger");
        }
	}
	// Update is called once per frame
	void Update () {
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Flamable>() != null 
            && other.gameObject.GetComponent<Flamable>().onFire
            && !onFire)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        if (flameSound != null)
            flameSound.Play();
        onFire = true;
        fireAnim.SetBool("onFire", onFire);
    }

    public void Extinguish()
    {
        
        if (!inextinguishable)
        {
            onFire = false;
            fireAnim.SetBool("onFire", onFire);
            if (flameSound != null)
                flameSound.Stop();
        }
    }
}