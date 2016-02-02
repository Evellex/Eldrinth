using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomEditor(typeof(CustomRenderResolution))]
	public class CustomRenderResolution_Editor : Editor
	{
		private SerializedProperty resolutionType;
		private SerializedProperty filterMode;
		private SerializedProperty customResolution;
		private SerializedProperty screenResolutionMultiplier;
		private SerializedProperty script;
		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script);

			label.text = "Resolution Mod Type";
			label.tooltip = "The type of resolution change applied";
			EditorGUILayout.PropertyField(resolutionType, label);

			label.text = "Filter Mode";
			label.tooltip = "The texture filtering mode used on the resulting image";
			EditorGUILayout.PropertyField(filterMode, label);

			if (resolutionType.intValue == (int)CustomRenderResolution.Type.CustomResolution)
			{
				label.text = "Resolution";
				label.tooltip = "Defines the absolute resolution that the scene is rendered at";
				EditorGUILayout.PropertyField(customResolution, label);
			}
			else if (resolutionType.intValue == (int)CustomRenderResolution.Type.ScreenResolutionMultiplier)
			{
				label.text = "Multiplier";
				label.tooltip = "Defines the factor of screen resolution that the scene is rendered at";
				EditorGUILayout.PropertyField(screenResolutionMultiplier, label);
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			script = serializedObject.FindProperty("m_Script");
			resolutionType = serializedObject.FindProperty("resolutionType");
			filterMode = serializedObject.FindProperty("filterMode");
			customResolution = serializedObject.FindProperty("customResolution");
			screenResolutionMultiplier = serializedObject.FindProperty("screenResolutionMultiplier");
		}
	}
}