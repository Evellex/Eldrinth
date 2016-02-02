using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GauntletPrefabSpawner : MonoBehaviour 
{
	[SerializeField]
	GameObject targetPrefab;
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
		if (!Application.isPlaying && targetPrefab != null && (prefabReference != targetPrefab || prefabInstance == null))
		{
			if (prefabInstance != null)
				DestroyImmediate(prefabInstance);
			prefabReference = targetPrefab;
			prefabInstance = GameObject.Instantiate(targetPrefab, transform.position, transform.rotation) as GameObject;
			prefabInstance.transform.parent = transform;
		}
	}
}
