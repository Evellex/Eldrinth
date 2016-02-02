using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
    public GameObject s1, s2;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SwapS()
    {
        s1.SetActive(false);
        s2.SetActive(true);
    }
}
