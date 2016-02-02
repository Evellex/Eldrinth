using Augmenta;
using System;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	internal class StandardShader_Editor : ShaderGUI
	{
		private MaterialProperty blendMode = null;

		private MaterialProperty cullMode = null;

		private MaterialProperty albedoMap = null;

		private MaterialProperty albedoColor = null;

		private MaterialProperty alphaCutoff = null;

		private MaterialProperty specularMap = null;

		private MaterialProperty specularColor = null;

		private MaterialProperty metallicMap = null;

		private MaterialProperty metallic = null;

		private MaterialProperty smoothness = null;

		private MaterialProperty bumpScale = null;

		private MaterialProperty bumpMap = null;

		private MaterialProperty occlusionStrength = null;

		private MaterialProperty occlusionMap = null;

		private MaterialProperty heigtMapScale = null;

		private MaterialProperty heightMap = null;

		private MaterialProperty emissionColorForRendering = null;

		private MaterialProperty emissionMap = null;

		private MaterialProperty temperatureMap = null;

		private MaterialProperty temperatureMin = null;

		private MaterialProperty temperatureMax = null;

		private MaterialProperty temperatureLookupMap = null;

		private MaterialProperty detailMask = null;

		private MaterialProperty detailAlbedoMap = null;

		private MaterialProperty detailNormalMapScale = null;

		private MaterialProperty detailNormalMap = null;

		private MaterialProperty uvSetSecondary = null;

		private MaterialEditor m_MaterialEditor;

		private WorkflowMode m_WorkflowMode = WorkflowMode.Specular;

		private ColorPickerHDRConfig m_ColorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1 / 99f, 3f);

		private Texture tempLookupTexture;

		private bool m_FirstTimeApply = true;

		public enum BlendMode
		{
			Opaque,
			Cutout,
			Fade,       // Old school alpha-blending mode, fresnel does not affect amount of transparency
			Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
		}

		public enum CullMode
		{
			Off = 0,
			Front = 1,
			Back = 2,
		}

		private enum WorkflowMode
		{
			Specular,
			Metallic,
			Dielectric
		}

		public static void SetupMaterial(Material material, BlendMode blendMode, CullMode cullMode)
		{
			switch (blendMode)
			{
				case BlendMode.Opaque:
					material.SetOverrideTag("RenderType", "");
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
					material.SetInt("_CullMode", (int)cullMode);
					material.SetInt("_ZWrite", 1);
					material.DisableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = -1;
					break;

				case BlendMode.Cutout:
					material.SetOverrideTag("RenderType", "TransparentCutout");
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
					material.SetInt("_CullMode", (int)cullMode);
					material.SetInt("_ZWrite", 1);
					material.EnableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 2450;
					break;

				case BlendMode.Fade:
					material.SetOverrideTag("RenderType", "Transparent");
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					material.SetInt("_CullMode", (int)cullMode);
					material.SetInt("_ZWrite", 0);
					material.DisableKeyword("_ALPHATEST_ON");
					material.EnableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 3000;
					break;

				case BlendMode.Transparent:
					material.SetOverrideTag("RenderType", "Transparent");
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					material.SetInt("_CullMode", (int)cullMode);
					material.SetInt("_ZWrite", 0);
					material.DisableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 3000;
					break;
			}
		}

		public void FindProperties(MaterialProperty[] props)
		{
			blendMode = FindProperty("_Mode", props);
			cullMode = FindProperty("_CullMode", props);
			albedoMap = FindProperty("_MainTex", props);
			albedoColor = FindProperty("_Color", props);
			alphaCutoff = FindProperty("_Cutoff", props);
			specularMap = FindProperty("_SpecGlossMap", props, false);
			specularColor = FindProperty("_SpecColor", props, false);
			metallicMap = FindProperty("_MetallicGlossMap", props, false);
			metallic = FindProperty("_Metallic", props, false);
			if (specularMap != null && specularColor != null)
				m_WorkflowMode = WorkflowMode.Specular;
			else if (metallicMap != null && metallic != null)
				m_WorkflowMode = WorkflowMode.Metallic;
			else
				m_WorkflowMode = WorkflowMode.Dielectric;
			smoothness = FindProperty("_Glossiness", props);
			bumpScale = FindProperty("_BumpScale", props);
			bumpMap = FindProperty("_BumpMap", props);
			heigtMapScale = FindProperty("_Parallax", props);
			heightMap = FindProperty("_ParallaxMap", props);
			occlusionStrength = FindProperty("_OcclusionStrength", props);
			occlusionMap = FindProperty("_OcclusionMap", props);
			emissionColorForRendering = FindProperty("_EmissionColor", props);
			emissionMap = FindProperty("_EmissionMap", props);
			temperatureMap = FindProperty("_TemperatureMap", props);
			temperatureMin = FindProperty("_TemperatureMin", props);
			temperatureMax = FindProperty("_TemperatureMax", props);
			temperatureLookupMap = FindProperty("_TemperatureLookupMap", props);
			detailMask = FindProperty("_DetailMask", props);
			detailAlbedoMap = FindProperty("_DetailAlbedoMap", props);
			detailNormalMapScale = FindProperty("_DetailNormalMapScale", props);
			detailNormalMap = FindProperty("_DetailNormalMap", props);
			uvSetSecondary = FindProperty("_UVSec", props);
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly

			if (tempLookupTexture == null)
				tempLookupTexture = Resources.Load("Augmenta/TemperatureLookup") as Texture;
			temperatureLookupMap.textureValue = tempLookupTexture;

			m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;

			ShaderPropertiesGUI(material);

			// Make sure that needed keywords are set up if we're switching some existing
			// material to a standard shader.
			if (m_FirstTimeApply)
			{
				SetMaterialKeywords(material, m_WorkflowMode);
				m_FirstTimeApply = false;
			}
		}

		public void ShaderPropertiesGUI(Material material)
		{
			// Use default labelWidth
			EditorGUIUtility.labelWidth = 0f;

			// Detect any changes to the material
			EditorGUI.BeginChangeCheck();
			{
				BlendModePopup();
				CullModePopup();

				// Primary properties
				GUILayout.Label(Styles.primaryMapsText, EditorStyles.boldLabel);
				DoAlbedoArea(material);
				DoSpecularMetallicArea();
				m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
				m_MaterialEditor.TexturePropertySingleLine(Styles.heightMapText, heightMap, heightMap.textureValue != null ? heigtMapScale : null);
				m_MaterialEditor.TexturePropertySingleLine(Styles.occlusionText, occlusionMap, occlusionMap.textureValue != null ? occlusionStrength : null);
				DoEmissionArea(material);
				DoTemperatureArea(material);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailMaskText, detailMask);
				EditorGUI.BeginChangeCheck();
				m_MaterialEditor.TextureScaleOffsetProperty(albedoMap);
				if (EditorGUI.EndChangeCheck())
					emissionMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset; // Apply the main texture scale and offset to the emission texture as well, for Enlighten's sake

				EditorGUILayout.Space();

				// Secondary properties
				GUILayout.Label(Styles.secondaryMapsText, EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailAlbedoText, detailAlbedoMap);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailNormalMapText, detailNormalMap, detailNormalMapScale);
				m_MaterialEditor.TextureScaleOffsetProperty(detailAlbedoMap);
				m_MaterialEditor.ShaderProperty(uvSetSecondary, Styles.uvSetLabel.text);
			}
			if (EditorGUI.EndChangeCheck())
			{
				foreach (var obj in blendMode.targets)
					MaterialChanged((Material)obj, m_WorkflowMode);
			}
		}

		public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
		{
			base.AssignNewShaderToMaterial(material, oldShader, newShader);

			if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
				return;

			BlendMode blendMode = BlendMode.Opaque;
			if (oldShader.name.Contains("/Transparent/Cutout/"))
			{
				blendMode = BlendMode.Cutout;
			}
			else if (oldShader.name.Contains("/Transparent/"))
			{
				// NOTE: legacy shaders did not provide physically based transparency
				// therefore Fade mode
				blendMode = BlendMode.Fade;
			}

			material.SetFloat("_Mode", (float)blendMode);

			DetermineWorkflow(MaterialEditor.GetMaterialProperties(new Material[] { material }));
			MaterialChanged(material, m_WorkflowMode);
		}

		internal void DetermineWorkflow(MaterialProperty[] props)
		{
			if (FindProperty("_SpecGlossMap", props, false) != null && FindProperty("_SpecColor", props, false) != null)
				m_WorkflowMode = WorkflowMode.Specular;
			else if (FindProperty("_MetallicGlossMap", props, false) != null && FindProperty("_Metallic", props, false) != null)
				m_WorkflowMode = WorkflowMode.Metallic;
			else
				m_WorkflowMode = WorkflowMode.Dielectric;
		}

		private static bool ShouldRadianceBeEnabled(Material material)
		{
			return material.GetColor("_EmissionColor").maxColorComponent > (0.1f / 255.0f);
		}

		private static bool ShouldTemperatureBeEnabled(Material material)
		{
			Texture texture = material.GetTexture("_TemperatureMap");
			if (texture == null)
			{
				return material.GetFloat("_TemperatureMax") >= 700 || material.GetFloat("_TemperatureMin") >= 700;
			}
			return true;
		}

		private static bool ShouldEmissionBeEnabled(Material material)
		{
			return ShouldRadianceBeEnabled(material) || ShouldTemperatureBeEnabled(material);
		}

		private static void SetMaterialKeywords(Material material, WorkflowMode workflowMode)
		{
			// Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
			// (MaterialProperty value might come from renderer material property block)
			SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap") || material.GetTexture("_DetailNormalMap"));
			if (workflowMode == WorkflowMode.Specular)
				SetKeyword(material, "_SPECGLOSSMAP", material.GetTexture("_SpecGlossMap"));
			else if (workflowMode == WorkflowMode.Metallic)
				SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap"));
			SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));
			SetKeyword(material, "_DETAIL_MULX2", material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap"));
			SetKeyword(material, "_EMISSION", ShouldEmissionBeEnabled(material));
			SetKeyword(material, "_RADIANCE", ShouldRadianceBeEnabled(material));
			SetKeyword(material, "_TEMPERATURE", ShouldTemperatureBeEnabled(material));

			// Setup lightmap emissive flags
			MaterialGlobalIlluminationFlags flags = material.globalIlluminationFlags;
			if ((flags & (MaterialGlobalIlluminationFlags.BakedEmissive | MaterialGlobalIlluminationFlags.RealtimeEmissive)) != 0)
			{
				flags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
				if (!ShouldEmissionBeEnabled(material))
					flags |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;

				material.globalIlluminationFlags = flags;
			}
		}

		private static void MaterialChanged(Material material, WorkflowMode workflowMode)
		{
			material.SetFloat("_TemperatureMax", Mathf.Clamp(material.GetFloat("_TemperatureMax"), 0, 10000));
			material.SetFloat("_TemperatureMin", Mathf.Clamp(material.GetFloat("_TemperatureMin"), 0, 10000));

			SetupMaterial(material, (BlendMode)material.GetFloat("_Mode"), (CullMode)material.GetFloat("_CullMode"));

			SetMaterialKeywords(material, workflowMode);
		}

		private static void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
				m.EnableKeyword(keyword);
			else
				m.DisableKeyword(keyword);
		}

		private void BlendModePopup()
		{
			EditorGUI.showMixedValue = blendMode.hasMixedValue;
			var mode = (BlendMode)blendMode.floatValue;

			EditorGUI.BeginChangeCheck();
			mode = (BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
			if (EditorGUI.EndChangeCheck())
			{
				m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
				blendMode.floatValue = (float)mode;
			}

			EditorGUI.showMixedValue = false;
		}

		private void CullModePopup()
		{
			EditorGUI.showMixedValue = cullMode.hasMixedValue;
			var mode = (UnityEngine.Rendering.CullMode)cullMode.floatValue;

			EditorGUI.BeginChangeCheck();
			mode = (UnityEngine.Rendering.CullMode)EditorGUILayout.Popup(Styles.cullMode, (int)mode, Styles.cullNames);
			if (EditorGUI.EndChangeCheck())
			{
				m_MaterialEditor.RegisterPropertyChangeUndo("Culling Mode");
				cullMode.floatValue = (float)mode;
			}

			EditorGUI.showMixedValue = false;
		}

		private void DoAlbedoArea(Material material)
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
			if (((BlendMode)material.GetFloat("_Mode") == BlendMode.Cutout))
			{
				m_MaterialEditor.ShaderProperty(alphaCutoff, Styles.alphaCutoffText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
			}
		}

		private void DoEmissionArea(Material material)
		{
			float brightness = emissionColorForRendering.colorValue.maxColorComponent;
			bool showHelpBox = !HasValidEmissiveKeyword(material);
			bool hadEmissionTexture = emissionMap.textureValue != null;

			// Texture and HDR color controls
			m_MaterialEditor.TexturePropertyWithHDRColor(Styles.emissionText, emissionMap, emissionColorForRendering, m_ColorPickerHDRConfig, false);

			// If texture was assigned and color was black set color to white
			if (emissionMap.textureValue != null && !hadEmissionTexture && brightness <= 0f)
				emissionColorForRendering.colorValue = Color.white;

			if (showHelpBox)
				EditorGUILayout.HelpBox(Styles.emissiveWarning.text, MessageType.Warning);
		}

		private void DoTemperatureArea(Material material)
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.temperatureText, temperatureMap, temperatureMin, temperatureMax);
			if (!HasValidTemperatureKeyword(material))
			{
				EditorGUILayout.HelpBox(Styles.temperatureWarning.text, MessageType.Warning);
			}

			// Dynamic Lightmapping mode
			if (ShouldShowGIControls())
			{
				EditorGUI.BeginDisabledGroup(!ShouldEmissionBeEnabled(material));
				m_MaterialEditor.LightmapEmissionProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
				EditorGUI.EndDisabledGroup();
			}
		}

		private bool ShouldShowGIControls()
		{
			return (emissionColorForRendering.colorValue.maxColorComponent > 0.0f || Mathf.Max(temperatureMin.floatValue, temperatureMax.floatValue) >= 700);
		}

		private void DoSpecularMetallicArea()
		{
			if (m_WorkflowMode == WorkflowMode.Specular)
			{
				if (specularMap.textureValue == null)
					m_MaterialEditor.TexturePropertyTwoLines(Styles.specularMapText, specularMap, specularColor, Styles.smoothnessText, smoothness);
				else
					m_MaterialEditor.TexturePropertySingleLine(Styles.specularMapText, specularMap);
			}
			else if (m_WorkflowMode == WorkflowMode.Metallic)
			{
				if (metallicMap.textureValue == null)
					m_MaterialEditor.TexturePropertyTwoLines(Styles.metallicMapText, metallicMap, metallic, Styles.smoothnessText, smoothness);
				else
					m_MaterialEditor.TexturePropertySingleLine(Styles.metallicMapText, metallicMap);
			}
		}

		private bool HasValidEmissiveKeyword(Material material)
		{
			// Material animation might be out of sync with the material keyword.
			// So if the emission support is disabled on the material, but the property blocks have a value that requires it, then we need to show a warning.
			// (note: (Renderer MaterialPropertyBlock applies its values to emissionColorForRendering))
			bool hasEmissionKeyword = material.IsKeywordEnabled("_EMISSION");
			if (!hasEmissionKeyword && ShouldEmissionBeEnabled(material))
				return false;
			else
				return true;
		}

		private bool HasValidTemperatureKeyword(Material material)
		{
			bool hasTemperatureKeyword = material.IsKeywordEnabled("_TEMPERATURE");
			if (!hasTemperatureKeyword && ShouldTemperatureBeEnabled(material))
				return false;
			else
				return true;
		}

		private static class Styles
		{
			public static readonly string[] blendNames = Enum.GetNames(typeof(BlendMode));
			public static readonly string[] cullNames = Enum.GetNames(typeof(UnityEngine.Rendering.CullMode));
			public static GUIStyle optionsButton = "PaneOptions";
			public static GUIContent uvSetLabel = new GUIContent("UV Set");
			public static GUIContent[] uvSetOptions = new GUIContent[] { new GUIContent("UV channel 0"), new GUIContent("UV channel 1") };

			public static string emptyTootip = "";
			public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Transparency (A)");
			public static GUIContent alphaCutoffText = new GUIContent("Alpha Cutoff", "Threshold for alpha cutoff");
			public static GUIContent specularMapText = new GUIContent("Specular", "Specular (RGB) and Smoothness (A)");
			public static GUIContent metallicMapText = new GUIContent("Metallic", "Metallic (R) and Smoothness (A)");
			public static GUIContent smoothnessText = new GUIContent("Smoothness", "");
			public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map");
			public static GUIContent heightMapText = new GUIContent("Height Map", "Height Map (G)");
			public static GUIContent occlusionText = new GUIContent("Occlusion", "Occlusion (G)");
			public static GUIContent emissionText = new GUIContent("Emission", "Emission (RGB)");
			public static GUIContent temperatureText = new GUIContent("Temperature", "Temperature (G), Min & Max (Kelvin)");
			public static GUIContent detailMaskText = new GUIContent("Detail Mask", "Mask for Secondary Maps (A)");
			public static GUIContent detailAlbedoText = new GUIContent("Detail Albedo x2", "Albedo (RGB) multiplied by 2");
			public static GUIContent detailNormalMapText = new GUIContent("Normal Map", "Normal Map");

			public static string whiteSpaceString = " ";
			public static string primaryMapsText = "Main Maps";
			public static string secondaryMapsText = "Secondary Maps";
			public static string renderingMode = "Rendering Mode";
			public static string cullMode = "Culling Mode";

			public static GUIContent emissiveWarning = new GUIContent("Emissive value is animated but the material has not been configured to support emissive. Please make sure the material itself has some amount of emissive.");
			public static GUIContent emissiveColorWarning = new GUIContent("Ensure emissive color is non-black for emission to have effect.");
			public static GUIContent temperatureWarning = new GUIContent("Temperature value is animated but the material has not been configured to support temperature. Please make sure the material itself has some amount of temperature.");
		}

#pragma warning disable 0414
#pragma warning restore 0414
	}
}