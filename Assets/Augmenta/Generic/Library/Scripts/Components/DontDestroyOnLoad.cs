using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Dont Destroy On Load")]
	public class DontDestroyOnLoad : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}