using UnityEngine;

namespace Augmenta
{
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	public class CustomRenderResolutionFinalStage : MonoBehaviour
	{
		[HideInInspector]
		public RenderTexture customTexture;
		[HideInInspector]
		public CustomRenderResolution creator = null;

		void Start()
		{
			if (creator == null)
			{
				Debug.LogError("CustomRenderResolutionFinalStage component must not be created on its own. Use CustomRenderResolution instead.");
				if (Application.isPlaying)
					Destroy(this);
				else
					DestroyImmediate(this);
			}
		}

		void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			if (Application.isPlaying)			
				Graphics.Blit(customTexture, (RenderTexture)null);			
		}
	}
}