using UnityEngine;
using System.Collections;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Agent")]
	public class Agent : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		int id;

		[SerializeField]
		[HideInInspector]
		string agentName = "";

		[SerializeField]
		[HideInInspector]
		string initialName = "#DEFAULT_NAME";

		static int firstFreeID;

		void Awake()
		{
			id = firstFreeID++;
			if(initialName != "")			
				SetName(initialName);
		}

		public int GetID()
		{
			return id;
		}

		public string GetName()
		{
			return agentName;
		}

		public void SetName(string newName)
		{
			agentName = newName;
		}
	}
}
