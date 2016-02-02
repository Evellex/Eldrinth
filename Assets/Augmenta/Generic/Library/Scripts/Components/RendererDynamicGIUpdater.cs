using UnityEngine;
using System.Collections;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Renderer Dynamic GI Updater")]
	public class RendererDynamicGIUpdater : MonoBehaviour
	{
		new Renderer renderer;

		void Start()
		{
			renderer = GetComponent<Renderer>();
		}

		void Update()
		{
			DynamicGI.UpdateMaterials(renderer);
		}
	}
}
