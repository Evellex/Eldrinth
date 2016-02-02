using Augmenta.Combat;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor.Combat
{
	[CustomEditor(typeof(DamageEventAction))]
	[CanEditMultipleObjects]
	public class DamageEventAction_Editor : Editor
	{
		private SerializedProperty settingsFlags;
		private SerializedProperty delayToAction;
		private SerializedProperty onTrigger;
		private SerializedProperty script;

		private SerializedProperty triggerOn;

		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script, true, new GUILayoutOption[0]);

			label.text = "Settings Flags";
			label.tooltip = "What settings this Event Action has.";
			EditorGUILayout.PropertyField(settingsFlags, label);

			label.text = "Delay To Action";
			label.tooltip = "The delay that is placed between the trigger, and the action.";
			EditorGUILayout.PropertyField(delayToAction, label);

			//===

			label.text = "Trigger On";
			label.tooltip = "The possible triggers for this EventAction";
			EditorGUILayout.PropertyField(triggerOn, label);

			//===

			label.text = "On Trigger";
			label.tooltip = "What events to call when this EventAction is triggered.";
			EditorGUILayout.PropertyField(onTrigger, label);

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			settingsFlags = serializedObject.FindProperty("settingsFlags");
			delayToAction = serializedObject.FindProperty("delayToAction");
			onTrigger = serializedObject.FindProperty("onTrigger");
			script = serializedObject.FindProperty("m_Script");
			triggerOn = serializedObject.FindProperty("triggerOn");
		}
	}
}