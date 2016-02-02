using UnityEngine;
using System.Collections;

public class SpawnPrefabDevice : Device {
    public GameObject prefabToSpawn;
    GameObject spawnedPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        spawnedPrefab = (GameObject) Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }

    public override void Deactivate()
    {
        Destroy(spawnedPrefab);
    }
}