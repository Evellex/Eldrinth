using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GauntletRandomPrefabSpawner : MonoBehaviour 
{
	[SerializeField]
	GameObject[] targetPrefab;
	[SerializeField]
	[HideInInspector]
	GameObject prefabInstance;
	[SerializeField]
	[HideInInspector]
	GameObject prefabReference;
	
	void Awake()
	{
		if (!Application.isPlaying)
		{
			if (prefabInstance != null)
				DestroyImmediate(prefabInstance);
			EnforcePrefabInstance();
		}
	}
	
	void Update()
	{
		EnforcePrefabInstance();
	}
	
	void EnforcePrefabInstance()
	{
		if (!Application.isPlaying && targetPrefab[0] != null && (prefabReference != targetPrefab[0] || prefabInstance == null))
		{
			if (prefabInstance != null)
				DestroyImmediate(prefabInstance);
			prefabReference = targetPrefab[0];
			prefabInstance = GameObject.Instantiate(targetPrefab[Random.Range(0,targetPrefab.Length)] , transform.position, transform.rotation) as GameObject;
			prefabInstance.transform.parent = transform;
		}
	}
}
