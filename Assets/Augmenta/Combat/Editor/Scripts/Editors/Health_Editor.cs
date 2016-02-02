using Augmenta.Combat;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor.Combat
{
	[CustomEditor(typeof(Health))]
	[CanEditMultipleObjects]
	public class Health_Editor : Editor
	{
		private SerializedProperty currentHealth;
		private SerializedProperty maximumHealth;
		private SerializedProperty initialHealth;
		private SerializedProperty responsibleAgent;
		private SerializedProperty damageReceivers;
		private SerializedProperty damageModifiers;
		private SerializedProperty factionController;
		private SerializedProperty script;

		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script);

			float healthValue = 1;
			if (Application.isPlaying)
				healthValue = currentHealth.floatValue;
			else
				healthValue = initialHealth.floatValue;

			float fractionalValue = healthValue / maximumHealth.floatValue;
			if (maximumHealth.floatValue <= 0)
				fractionalValue = 1;
			float percentValue = Mathf.RoundToInt(fractionalValue * 100.0f);

			string text = "Current Health: " + healthValue.ToString() + " (" + percentValue.ToString() + "%)";
			EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), fractionalValue, text);

			label.text = "Maximum Health";
			label.tooltip = "The general maximum health this instance can have.";
			EditorGUILayout.PropertyField(maximumHealth, label);

			label.text = "Initial Health";
			label.tooltip = "The amount of health this instance starts with.";
			EditorGUILayout.PropertyField(initialHealth, label);

			label.text = "Responsible Agent";
			label.tooltip = "The agent responsible for this Health script (optional)";
			EditorGUILayout.PropertyField(responsibleAgent, label);

			label.text = "Damage Receivers";
			label.tooltip = "The rigidbodies that will send damage events to this instance.";
			EditorGUILayout.PropertyField(damageReceivers, label, true);

			label.text = "Damage Modifiers";
			label.tooltip = "The modifications to apply to damage before it is applied to this instance.";
			EditorGUILayout.PropertyField(damageModifiers, label, true);

			label.text = "Faction Controller";
			label.tooltip = "Defines what faction this health belongs to for the purposes of friendly fire.";
			EditorGUILayout.PropertyField(factionController, label);

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			script = serializedObject.FindProperty("m_Script");

			currentHealth = serializedObject.FindProperty("currentHealth");
			maximumHealth = serializedObject.FindProperty("maximumHealth");
			initialHealth = serializedObject.FindProperty("initialHealth");
			responsibleAgent = serializedObject.FindProperty("responsibleAgent");
			damageReceivers = serializedObject.FindProperty("damageReceivers");
			damageModifiers = serializedObject.FindProperty("damageModifiers");
			factionController = serializedObject.FindProperty("factionController");
		}
	}
}