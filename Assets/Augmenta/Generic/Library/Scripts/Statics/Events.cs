using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("")]
	[System.Obsolete(" The Events class is deprecated. Please use the UnityEvents class.")]
	public class Events : MonoBehaviour
	{
		private static Events instance = null;

		public static event System.Action On_Update;

		public static event System.Action On_FixedUpdate;

		public static event System.Action On_LateUpdate;

		[RuntimeInitializeOnLoadMethod]
		private static void VerifyInstance()
		{
			instance = new GameObject("Augmenta Events Holder").AddComponent<Events>();
			instance.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
			DontDestroyOnLoad(instance);
		}

		private void Update()
		{
			if (On_Update != null)
				On_Update.Invoke();
		}

		private void FixedUpdate()
		{
			if (On_FixedUpdate != null)
				On_FixedUpdate.Invoke();
		}

		private void LateUpdate()
		{
			if (On_LateUpdate != null)
				On_LateUpdate.Invoke();
		}
	}
}