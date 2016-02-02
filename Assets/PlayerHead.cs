using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SphereCollider))]

public class PlayerHead : MonoBehaviour {
    SphereCollider headCol;
	// Use this for initialization
	void Start () {
        headCol = GetComponent<SphereCollider>();
        headCol.isTrigger = true;
	}

}