using UnityEngine;
using System.Collections;
[RequireComponent(typeof(HandController))]
public class CrossbowShoot : MonoBehaviour {

    public GameObject boltPrefab;
    public Transform shootpoint;
    AudioSource boltFireSound;
    HandController hc;
    /*SteamVR_Controller.Device device;
    SteamVR_TrackedObject trackedObj;

    public SteamVR_TrackedObject.EIndex index = SteamVR_TrackedObject.EIndex.None;*/


    void Start()
    {
        boltFireSound = GetComponent<AudioSource>();
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
        //device = SteamVR_Controller.Input((int)trackedObj.index);
        hc = GetComponent<HandController>();
    }

    void Update()
    {
        
        if (hc.triggerPress)
        {
            boltFireSound.Play();
            GameObject go = Instantiate(boltPrefab, shootpoint.position, shootpoint.rotation * boltPrefab.transform.rotation) as GameObject;
        }
        /*if ( Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = Instantiate(boltPrefab, shootpoint.position, transform.rotation) as GameObject;
        }*/

    }
}
