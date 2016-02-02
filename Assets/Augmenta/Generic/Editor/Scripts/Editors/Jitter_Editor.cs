using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomEditor(typeof(Jitter))]
	[CanEditMultipleObjects]
	public class Jitter_Editor : Editor
	{
		private SerializedProperty script;

		private SerializedProperty targetType;
		private SerializedProperty speed;
		private SerializedProperty smoothness;
		private SerializedProperty overallLimitType;
		private SerializedProperty movementLimitType;
		private SerializedProperty overallMagnitudeLimit;
		private SerializedProperty movementMagnitudeLimit;

		private SerializedProperty onValueChangeFloat;
		private SerializedProperty f_minimumOverall;
		private SerializedProperty f_maximumOverall;
		private SerializedProperty f_minimumChange;
		private SerializedProperty f_maximumChange;

		private SerializedProperty onValueChangeVector3;
		private SerializedProperty v3_minimumOverall;
		private SerializedProperty v3_maximumOverall;
		private SerializedProperty v3_minimumChange;
		private SerializedProperty v3_maximumChange;

		private GUIContent label = new GUIContent();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(script);

			label.text = "Target Type";
			label.tooltip = "The type of value that will be jittered.";
			EditorGUILayout.PropertyField(targetType, label);

			EditorGUI.BeginDisabledGroup(targetType.hasMultipleDifferentValues);

			label.text = "Speed";
			label.tooltip = "The speed with which the value will travel (units/second)";
			EditorGUILayout.PropertyField(speed, label);

			EditorGUI.BeginDisabledGroup(true);

			label.text = "Smoothness";
			label.tooltip = "The smoothness of the path that the value follows.";
			EditorGUILayout.PropertyField(smoothness, label);

			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			label.text = "Value Limit Type";
			label.tooltip = "How the range of the value is defined.";
			EditorGUILayout.PropertyField(overallLimitType, label);

			EditorGUI.indentLevel++;
			if (overallLimitType.intValue == (int)Jitter.LimitType.Magnitude)
			{
				label.text = "Value Magnitude Limit";
				label.tooltip = "The distance from the origin the value is allowed to be.";
				EditorGUILayout.PropertyField(overallMagnitudeLimit, label);
			}
			else if (overallLimitType.intValue == (int)Jitter.LimitType.Range)
			{
				if (targetType.intValue == (int)Jitter.TargetType.Float)
				{
					label.text = "Value Minimum";
					label.tooltip = "The minimum  value limit.";
					EditorGUILayout.PropertyField(f_minimumOverall, label);

					label.text = "Value Maximum";
					label.tooltip = "The maximum value limit.";
					EditorGUILayout.PropertyField(f_maximumOverall, label);
				}
				else if (targetType.intValue == (int)Jitter.TargetType.Vector3)
				{
					label.text = "Value Minimum";
					label.tooltip = "The minimum  value limit.";
					EditorGUILayout.PropertyField(v3_minimumOverall, label);

					label.text = "Value Maximum";
					label.tooltip = "The maximum  value limit.";
					EditorGUILayout.PropertyField(v3_maximumOverall, label);
				}
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();

			label.text = "Change Limit Type";
			label.tooltip = "How the range of any change to the value is defined.";
			int movementLimitTypeEditVal = (int)(Jitter.LimitType)EditorGUILayout.EnumPopup(label, (Jitter.LimitType)movementLimitType.intValue);

			EditorGUI.indentLevel++;
			if (movementLimitType.intValue == (int)Jitter.LimitType.Magnitude)
			{
				label.text = "Change Magnitude Limit";
				label.tooltip = "The distance from the value the change is allowed to be.";
				float changeMagnitudeLimitEditVal = EditorGUILayout.FloatField(label, movementMagnitudeLimit.floatValue);
			}
			else if (movementLimitType.intValue == (int)Jitter.LimitType.Range)
			{
				if (targetType.intValue == (int)Jitter.TargetType.Float)
				{
					label.text = "Change Minimum";
					label.tooltip = "The minimum the value can change by.";
					EditorGUILayout.PropertyField(f_minimumChange, label);

					label.text = "Change Maximum";
					label.tooltip = "The maximum the value can change by.";
					EditorGUILayout.PropertyField(f_maximumChange, label);
				}
				else if (targetType.intValue == (int)Jitter.TargetType.Vector3)
				{
					label.text = "Change Minimum";
					label.tooltip = "The minimum the value can change by.";
					EditorGUILayout.PropertyField(v3_minimumChange, label);

					label.text = "Change Maximum";
					label.tooltip = "The maximum the value can change by.";
					EditorGUILayout.PropertyField(v3_maximumChange, label);
				}
			}
			EditorGUI.indentLevel--;

			if (targetType.intValue == (int)Jitter.TargetType.Float)
			{
				EditorGUILayout.PropertyField(onValueChangeFloat);
			}
			else if (targetType.intValue == (int)Jitter.TargetType.Vector3)
			{
				EditorGUILayout.PropertyField(onValueChangeVector3);
			}

			EditorGUI.EndDisabledGroup();

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			script = serializedObject.FindProperty("m_Script");

			targetType = serializedObject.FindProperty("targetType");
			speed = serializedObject.FindProperty("speed");
			smoothness = serializedObject.FindProperty("smoothness");
			overallLimitType = serializedObject.FindProperty("overallLimitType");
			movementLimitType = serializedObject.FindProperty("movementLimitType");
			overallMagnitudeLimit = serializedObject.FindProperty("overallMagnitudeLimit");
			movementMagnitudeLimit = serializedObject.FindProperty("movementMagnitudeLimit");

			onValueChangeFloat = serializedObject.FindProperty("onValueChangeFloat");
			f_minimumOverall = serializedObject.FindProperty("f_minimumOverall");
			f_maximumOverall = serializedObject.FindProperty("f_maximumOverall");
			f_minimumChange = serializedObject.FindProperty("f_minimumChange");
			f_maximumChange = serializedObject.FindProperty("f_maximumChange");

			onValueChangeVector3 = serializedObject.FindProperty("onValueChangeVector3");
			v3_minimumOverall = serializedObject.FindProperty("v3_minimumOverall");
			v3_maximumOverall = serializedObject.FindProperty("v3_maximumOverall");
			v3_minimumChange = serializedObject.FindProperty("v3_minimumChange");
			v3_maximumChange = serializedObject.FindProperty("v3_maximumChange");
		}
	}
}