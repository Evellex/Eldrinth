using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomEditor(typeof(RandomInitialTransform))]
	[CanEditMultipleObjects]
	public class RandomInitialTransform_Editor : Editor
	{
		private SerializedProperty script;

		private SerializedProperty applyAtRuntime;
		private SerializedProperty randomRotation;
		private SerializedProperty rotationType;
		private SerializedProperty rotationSpace;
		private SerializedProperty randomScale;
		private SerializedProperty scaleDistribution;
		private SerializedProperty rotationAxis;
		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script);

			label.text = "Apply Now";
			label.tooltip = "Applies selected random transform values immediately in the editor.";
			if (GUILayout.Button(label))
				(target as RandomInitialTransform).ApplyRandomTransform();

			label.text = "Apply At Runtime";
			label.tooltip = "If true, applies selected random transform values when the object is created at runtime.";
			EditorGUILayout.PropertyField(applyAtRuntime, label);

			EditorGUIExt.BeginChangeCheckMulti(randomRotation);
			label.text = "Random Rotation";
			label.tooltip = "If true, applies random rotation using selected values.";
			bool doRandomRotation = EditorGUILayout.BeginToggleGroup(label, randomRotation.boolValue);
			if (EditorGUIExt.EndChangeCheckMulti())
				randomRotation.boolValue = doRandomRotation;

			label.text = "Rotation Axis";
			label.tooltip = "Defines the axis around which the object applies a random rotation.";
			EditorGUILayout.PropertyField(rotationType, label);

			EditorGUI.BeginDisabledGroup(rotationType.hasMultipleDifferentValues);

			if (rotationType.intValue != (int)RandomInitialTransform.Axes.AllAxes)
			{
				label.text = "Rotation Space";
				label.tooltip = "Defines the space in which the random rotation axis is defined.";
				EditorGUILayout.PropertyField(rotationSpace, label);
			}

			if (rotationType.intValue == (int)RandomInitialTransform.Axes.CustomAxis)
			{
				EditorGUIExt.BeginChangeCheckMulti(rotationAxis);
				label.text = "Custom Rotation Axis";
				label.tooltip = "Defines the axis around which the object applies a random rotation.";
				EditorGUILayout.PropertyField(rotationAxis, label);
			}
			EditorGUILayout.EndToggleGroup();

			EditorGUI.EndDisabledGroup();

			EditorGUIExt.BeginChangeCheckMulti(randomScale);
			label.text = "Random Scale";
			label.tooltip = "If true, applies random scale using selected values.";
			bool doRandomScale = EditorGUILayout.BeginToggleGroup(label, randomScale.boolValue);
			if (EditorGUIExt.EndChangeCheckMulti())
				randomScale.boolValue = doRandomScale;

			label.text = "Scale Distribution";
			label.tooltip = "Defines the probability of getting particular uniform scales (Y Axis is scale, picks random point on curve).";
			EditorGUILayout.PropertyField(scaleDistribution, label);

			EditorGUILayout.EndToggleGroup();

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			script = serializedObject.FindProperty("m_Script");

			applyAtRuntime = serializedObject.FindProperty("applyAtRuntime");
			randomRotation = serializedObject.FindProperty("randomRotation");
			rotationType = serializedObject.FindProperty("rotationType");
			rotationSpace = serializedObject.FindProperty("rotationSpace");
			randomScale = serializedObject.FindProperty("randomScale");
			scaleDistribution = serializedObject.FindProperty("scaleDistribution");
			rotationAxis = serializedObject.FindProperty("rotationAxis");
		}
	}
}