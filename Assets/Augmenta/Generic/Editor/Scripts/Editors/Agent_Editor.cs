using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomEditor(typeof(Agent))]
	[CanEditMultipleObjects]
	public class Agent_Editor : Editor
	{
		private SerializedProperty id;
		private SerializedProperty agentName;
		private SerializedProperty initialName;
		private SerializedProperty script;
		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script);

			if (Application.isPlaying)
			{
				EditorGUILayout.LabelField("ID", EditorStyles.boldLabel);
				EditorGUIExt.BeginChangeCheckMulti(id);
				EditorGUI.indentLevel++;
				EditorGUILayout.SelectableLabel(id.intValue.ToString());
				EditorGUIExt.EndChangeCheckMulti();
				EditorGUI.indentLevel--;

				EditorGUILayout.LabelField("Name", EditorStyles.boldLabel);
				EditorGUIExt.BeginChangeCheckMulti(agentName);
				EditorGUI.indentLevel++;
				EditorGUILayout.SelectableLabel(agentName.stringValue);
				EditorGUIExt.EndChangeCheckMulti();
				EditorGUI.indentLevel--;
			}
			else
			{
				EditorGUILayout.LabelField("No ID in Edit Mode.");

				EditorGUIExt.BeginChangeCheckMulti(initialName);
				label.text = "Initial Name";
				label.tooltip = "A name to assign to the agent upon creation.";
				string nameValue = EditorGUILayout.TextField(label, initialName.stringValue);
				if (EditorGUIExt.EndChangeCheckMulti())
					initialName.stringValue = nameValue;
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			script = serializedObject.FindProperty("m_Script");
			id = serializedObject.FindProperty("id");
			agentName = serializedObject.FindProperty("agentName");
			initialName = serializedObject.FindProperty("initialName");
		}
	}
}