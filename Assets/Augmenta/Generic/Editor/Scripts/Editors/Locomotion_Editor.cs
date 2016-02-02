using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomEditor(typeof(Locomotion))]
	[CanEditMultipleObjects]
	public class Locomotion_Editor : Editor
	{
		private SerializedProperty rigidbody;
		private SerializedProperty capCollider;
		private SerializedProperty acceleration;
		private SerializedProperty maxSpeed;
		private SerializedProperty maxStepSize;
		private SerializedProperty maxWalkingAngle;
		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			label.text = "Rigidbody";
			label.tooltip = "The rigidbody used the the locomotion.";
			EditorGUILayout.PropertyField(rigidbody, label);

			label.text = "Capsule Collider";
			label.tooltip = "The capsule collider used the the locomotion.";
			EditorGUILayout.PropertyField(capCollider, label);

			label.text = "Acceleration";
			label.tooltip = "The acceleration of the locomotion (m*s^-2)";
			EditorGUILayout.PropertyField(acceleration, label);

			label.text = "Max Speed";
			label.tooltip = "The maximum speed locomotion (m*s^-1)";
			EditorGUILayout.PropertyField(maxSpeed, label);

			label.text = "Max Step Height";
			label.tooltip = "The maximum height of step the locomotion can traverse (m)";
			EditorGUILayout.PropertyField(maxStepSize, label);

			label.text = "Max Walking Angle";
			label.tooltip = "The maximum angle the locomotion can walk on (mτ)";
			EditorGUILayout.PropertyField(maxWalkingAngle, label);

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			rigidbody = serializedObject.FindProperty("rigidbody");
			capCollider = serializedObject.FindProperty("capCollider");
			acceleration = serializedObject.FindProperty("acceleration");
			maxSpeed = serializedObject.FindProperty("maxSpeed");
			maxStepSize = serializedObject.FindProperty("maxStepSize");
			maxWalkingAngle = serializedObject.FindProperty("maxWalkingAngle");
		}
	}
}