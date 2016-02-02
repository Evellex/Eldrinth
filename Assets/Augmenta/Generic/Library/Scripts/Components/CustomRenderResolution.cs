using UnityEngine;

namespace Augmenta
{
	[ExecuteInEditMode]
	[AddComponentMenu("Augmenta/Custom Render Resolution")]
	public class CustomRenderResolution : MonoBehaviour
	{
		[SerializeField]
		Type resolutionType;
		[SerializeField]
		FilterMode filterMode = FilterMode.Bilinear;
		FilterMode filterModeLast;
		[SerializeField]
		Int2 customResolution = new Int2(1920, 1080);
		Int2 customResolutionLast;

		[SerializeField]
		float screenResolutionMultiplier = 1.0f;

		float screenResolutionMultiplierLast;

		RenderTexture customTexture;

		Camera finalPassCamera;
		Camera targetCamera;
		CustomRenderResolutionFinalStage subScript;

		public enum Type
		{
			CustomResolution,
			ScreenResolutionMultiplier,
		}

		void Start()
		{
			if(Application.isPlaying)
			{
				if (!(targetCamera = GetComponent<Camera>()))
				{
					LogCameraError();
					return;
				}
				GameObject newChild = new GameObject("Final Pass Camera");
				newChild.transform.parent = transform;
				//newChild.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
				finalPassCamera = newChild.AddComponent<Camera>();
				finalPassCamera.CopyFrom(targetCamera);
				finalPassCamera.clearFlags = CameraClearFlags.Nothing;
				finalPassCamera.backgroundColor = new Color(1, 1, 1, 0);
				finalPassCamera.cullingMask = 0;
				finalPassCamera.orthographic = true;
				finalPassCamera.depth = targetCamera.depth + 0.01f;
				finalPassCamera.hdr = targetCamera.hdr;
				finalPassCamera.renderingPath = RenderingPath.Forward;
				finalPassCamera.useOcclusionCulling = false;
				subScript = newChild.AddComponent<CustomRenderResolutionFinalStage>();
				subScript.creator = this;
				BuildTexture();
			}
			else
			{
				if (!GetComponent<Camera>())
				{
					LogCameraError();
					return;
				}
			}
		}

		void LogCameraError()
		{
			Debug.Log("Script \"CustomRenderResolution\" requires a camera to be attached to the same GameObject! Removing script...",gameObject);
		}

		void Update()
		{
			if (Application.isPlaying)
			{
				if (targetCamera == null)
				{
					LogCameraError();
					return;
				}
				if (resolutionType == Type.ScreenResolutionMultiplier && screenResolutionMultiplierLast != screenResolutionMultiplier)				
					BuildTexture();				
				if (resolutionType == Type.CustomResolution && customResolutionLast != customResolution)				
					BuildTexture();				
				if (filterMode != filterModeLast)				
					BuildTexture();				
			}
		}

		void BuildTexture()
		{
			if(Application.isPlaying)
			{
				if (resolutionType == Type.CustomResolution)
				{
					customResolution.x = Mathf.Max(1, customResolution.x);
					customResolution.x = Mathf.Min(Screen.width * 4, customResolution.x);
					customResolution.y = Mathf.Max(1, customResolution.y);
					customResolution.y = Mathf.Min(Screen.height * 4, customResolution.y);
					customTexture = new RenderTexture(customResolution.x, customResolution.y, 32);
				}
				else if (resolutionType == Type.ScreenResolutionMultiplier)
				{
					screenResolutionMultiplier = Mathf.Max(0, screenResolutionMultiplier);
					screenResolutionMultiplier = Mathf.Min(4, screenResolutionMultiplier);
					int width = Mathf.Max(1, Mathf.FloorToInt(screenResolutionMultiplier * Screen.width));
					int height = Mathf.Max(1, Mathf.FloorToInt(screenResolutionMultiplier * Screen.height));
					customTexture = new RenderTexture(width, height, 32);
				}
				customTexture.filterMode = filterMode;
				customTexture.name = "Custom Render Resolution Texture";
				customTexture.Create();
				targetCamera.targetTexture = customTexture;
				subScript.customTexture = customTexture;

				customResolutionLast = customResolution;
				filterModeLast = filterMode;
				screenResolutionMultiplierLast = screenResolutionMultiplier;
			}
		}
	}
}