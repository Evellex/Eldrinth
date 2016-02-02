using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("")]
	public class UnityEvents : MonoBehaviour
	{
		private static UnityEvents instance = null;

		public static event System.Action On_Update;

		public static event System.Action On_FixedUpdate;

		public static event System.Action On_LateUpdate;

		public static event System.Action<int> On_LevelWasLoaded;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void VerifyInstance()
		{
			instance = new GameObject("Augmenta Unity Event Receiver").AddComponent<UnityEvents>();
			instance.gameObject.hideFlags = HideFlags.NotEditable;
			DontDestroyOnLoad(instance);
		}

		private void OnLevelWasLoaded(int levelIndex)
		{
			if (On_LevelWasLoaded != null)
				On_LevelWasLoaded.Invoke(levelIndex);
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