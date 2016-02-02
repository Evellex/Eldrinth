using Augmenta.Combat;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor.Combat
{
	[CustomEditor(typeof(HealthEventAction))]
	[CanEditMultipleObjects]
	public class HealthEventAction_Editor : Editor
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

			EditorGUILayout.PropertyField(script);

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
			script = serializedObject.FindProperty("m_Script");

			settingsFlags = serializedObject.FindProperty("settingsFlags");
			delayToAction = serializedObject.FindProperty("delayToAction");
			onTrigger = serializedObject.FindProperty("onTrigger");

			triggerOn = serializedObject.FindProperty("triggerOn");
		}
	}
}