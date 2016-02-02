using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Object Pool Initialiser")]
	public class ObjectPoolInitialiser : MonoBehaviour
	{
		[SerializeField]
		private List<GameObject> templates;

		[SerializeField]
		private int count = 0;

		[SerializeField]
		private bool dontDepositOnLoad = false;

		private void Awake()
		{
			foreach (GameObject template in templates)
			{
				ObjectPooler.InitialisePool(template, count, dontDepositOnLoad);
			}
		}

		private void OnValidate()
		{
			if (count < 0)
				count = 0;
		}
	}
}