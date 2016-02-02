using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomEditor(typeof(TransformMotion))]
	[CanEditMultipleObjects]
	public class TransformMotion_Editor : Editor
	{
		private SerializedProperty script;

		private SerializedProperty updateType;
		private SerializedProperty rotate;
		private SerializedProperty rotationSpeed;
		private SerializedProperty rotationAxis;
		private SerializedProperty rotationSpace;
		private SerializedProperty rotationUnit;
		private SerializedProperty translate;
		private SerializedProperty translationSpeed;
		private SerializedProperty translationAxis;
		private SerializedProperty translationSpace;
		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script);

			label.text = "Update Type";
			label.tooltip = "The update loop that this motion occurs in.";
			EditorGUILayout.PropertyField(updateType, label);

			EditorGUIExt.BeginChangeCheckMulti(rotate);
			label.text = "Rotation";
			label.tooltip = "Does this transform have rotation motion?";
			bool rotationMotion = EditorGUILayout.BeginToggleGroup(label, rotate.boolValue);
			if (EditorGUIExt.EndChangeCheckMulti())
				rotate.boolValue = rotationMotion;

			label.text = "Rotation Axis";
			label.tooltip = "The axis on which this transform rotates.";
			EditorGUILayout.PropertyField(rotationAxis, label);

			label.text = "Rotation Speed";
			label.tooltip = "The speed at which this transform rotates, in the chosen units per second";
			EditorGUILayout.PropertyField(rotationSpeed, label);

			label.text = "Rotation Space";
			label.tooltip = "Defines the space in which the rotation axis is defined.";
			EditorGUILayout.PropertyField(rotationSpace, label);

			label.text = "Rotation Unit";
			label.tooltip = "Defines the unit of the rotation speed.";
			EditorGUILayout.PropertyField(rotationUnit, label);

			EditorGUILayout.EndToggleGroup();

			EditorGUIExt.BeginChangeCheckMulti(translate);
			label.text = "Translation";
			label.tooltip = "Does this transform have translation motion?";
			bool translationMotion = EditorGUILayout.BeginToggleGroup(label, translate.boolValue);
			if (EditorGUIExt.EndChangeCheckMulti())
				translate.boolValue = translationMotion;

			label.text = "Translation Axis";
			label.tooltip = "The axis on which this transform translates.";
			EditorGUILayout.PropertyField(translationAxis, label);

			label.text = "Translation Speed";
			label.tooltip = "The speed at which this transform translates, in units/s";
			EditorGUILayout.PropertyField(translationSpeed, label);

			label.text = "Translation Space";
			label.tooltip = "Defines the space in which the translation axis is defined.";
			EditorGUILayout.PropertyField(translationSpace, label);

			EditorGUILayout.EndToggleGroup();

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			script = serializedObject.FindProperty("m_Script");

			updateType = serializedObject.FindProperty("updateType");
			rotate = serializedObject.FindProperty("rotate");
			rotationSpeed = serializedObject.FindProperty("rotationSpeed");
			rotationAxis = serializedObject.FindProperty("rotationAxis");
			rotationSpace = serializedObject.FindProperty("rotationSpace");
			rotationUnit = serializedObject.FindProperty("rotationUnit");
			translate = serializedObject.FindProperty("translate");
			translationSpeed = serializedObject.FindProperty("translationSpeed");
			translationAxis = serializedObject.FindProperty("translationAxis");
			translationSpace = serializedObject.FindProperty("translationSpace");
		}
	}
}