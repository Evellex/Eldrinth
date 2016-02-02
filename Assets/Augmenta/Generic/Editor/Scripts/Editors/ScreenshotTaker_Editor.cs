using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomEditor(typeof(ScreenshotTaker))]
	[CanEditMultipleObjects]
	public class ScreenshotTaker_Editor : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (GUILayout.Button("Take Screenshot Now"))
			{
				(target as ScreenshotTaker).TakeScreenshot();
			}

			DrawDefaultInspector();

			serializedObject.ApplyModifiedProperties();
		}
	}
}