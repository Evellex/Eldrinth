using Augmenta;
using Augmenta.Combat;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor.Combat
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Damage))]
	public class Damage_Editor : Editor
	{
		private SerializedProperty damageType;
		private SerializedProperty damage;
		private SerializedProperty friendlyFire;
		private SerializedProperty sourceFaction;
		private SerializedProperty damageIdentifier;
		private SerializedProperty responsibleAgent;

		private SerializedProperty damageModifiers;

		private SerializedProperty physical;
		private SerializedProperty triggerType;
		private SerializedProperty continuous;
		private SerializedProperty repetitionDelay;
		private SerializedProperty script;

		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script);

			label.text = "Damage Type";
			label.tooltip = "Defines the type of damage applied.";
			EditorGUILayout.PropertyField(damageType, label);

			label.text = "Damage Done";
			label.tooltip = "How much damage is applied";
			EditorGUILayout.PropertyField(damage, label);

			label.text = "Source Faction";
			label.tooltip = "The Faction that this damage originates from";
			EditorGUILayout.PropertyField(sourceFaction, label);

			label.text = "Friendly Fire";
			label.tooltip = "Friendly fire setting for this damage";
			EditorGUILayout.PropertyField(friendlyFire, label);

			label.text = "Damage Identifier";
			label.tooltip = "The name used for the weapon.";
			EditorGUILayout.PropertyField(damageIdentifier, label);

			label.text = "Responsible Agent";
			label.tooltip = "The Agent responsible for this damage (can be blank)";
			EditorGUILayout.PropertyField(responsibleAgent, label);

			label.text = "Damage Modifiers";
			label.tooltip = "Modifications to apply to this damage.";
			EditorGUILayout.PropertyField(damageModifiers, label, true);

			EditorGUIExt.BeginChangeCheckMulti(physical);
			label.text = "Physical";
			label.tooltip = "Is the damage triggered by the physics system?";
			bool isPhysical = EditorGUILayout.BeginToggleGroup(label, physical.boolValue);
			if (EditorGUIExt.EndChangeCheckMulti())
				physical.boolValue = isPhysical;

			EditorGUI.indentLevel += 1;

			EditorGUIExt.BeginChangeCheckMulti(continuous);
			label.text = "Continous";
			label.tooltip = "Is the damage applied repetitively over time?";
			bool continuousBool = EditorGUILayout.BeginToggleGroup(label, continuous.boolValue);
			if (EditorGUIExt.EndChangeCheckMulti())
				continuous.boolValue = continuousBool;

			EditorGUI.indentLevel += 1;

			EditorGUIExt.BeginChangeCheckMulti(repetitionDelay);
			label.text = "Repetition Delay";
			label.tooltip = "Period of time between individual damage applications";
			float period = EditorGUILayout.FloatField(label, repetitionDelay.floatValue);
			if (EditorGUIExt.EndChangeCheckMulti())
				repetitionDelay.floatValue = period;

			EditorGUILayout.EndToggleGroup();
			EditorGUI.indentLevel -= 1;

			EditorGUIExt.BeginChangeCheckMulti(triggerType);
			label.text = "Trigger On:";
			label.tooltip = "What events trigger the damage application?";
			int triggerValue = (int)(PhysicsTypeFlags)EditorGUILayout.EnumMaskField(label, (PhysicsTypeFlags)triggerType.intValue);
			if (EditorGUIExt.EndChangeCheckMulti())
				triggerType.intValue = triggerValue;

			EditorGUILayout.EndToggleGroup();
			EditorGUI.indentLevel -= 1;

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			script = serializedObject.FindProperty("m_Script");

			damageType = serializedObject.FindProperty("damageType");
			damage = serializedObject.FindProperty("damageToApply");
			continuous = serializedObject.FindProperty("continuous");
			triggerType = serializedObject.FindProperty("triggerType");
			damageIdentifier = serializedObject.FindProperty("damageIdentifier");
			responsibleAgent = serializedObject.FindProperty("responsibleAgent");

			damageModifiers = serializedObject.FindProperty("damageModifiers");

			physical = serializedObject.FindProperty("physical");
			repetitionDelay = serializedObject.FindProperty("repetitionDelay");
			friendlyFire = serializedObject.FindProperty("friendlyFire");
			sourceFaction = serializedObject.FindProperty("sourceFaction");
		}
	}
}