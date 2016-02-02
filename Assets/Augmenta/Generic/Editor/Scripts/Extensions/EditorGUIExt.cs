using UnityEditor;

namespace AugmentaEditor
{
	public static class EditorGUIExt
	{
		public static void BeginChangeCheckMulti(SerializedProperty property)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
		}

		public static bool EndChangeCheckMulti()
		{
			EditorGUI.showMixedValue = false;
			return EditorGUI.EndChangeCheck();
		}
	}
}