using Augmenta;
using Augmenta.Combat;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor.Combat
{
	[CustomEditor(typeof(FactionMember))]
	[CanEditMultipleObjects]
	public class FactionMember_Editor : Editor
	{
		private SerializedProperty currentFactions;
		private SerializedProperty script;
		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script);

			label.text = "Player ID";
			label.tooltip = "The ID of the player this component listens for.";
			EditorGUILayout.PropertyField(currentFactions, label);

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			script = serializedObject.FindProperty("m_Script");
			currentFactions = serializedObject.FindProperty("currentFactions");
		}
	}
}