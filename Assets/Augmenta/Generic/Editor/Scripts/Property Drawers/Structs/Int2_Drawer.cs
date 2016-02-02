using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomPropertyDrawer(typeof(Int2))]
	public class Int2_Drawer : PropertyDrawer
	{
		private GUIContent label = new GUIContent();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty x = property.FindPropertyRelative("x");
			SerializedProperty y = property.FindPropertyRelative("y");

			this.label.text = ObjectNames.NicifyVariableName(property.name);
			this.label.tooltip = property.tooltip;

			Vector2 newHolderValue = new Vector2(x.intValue, y.intValue);
			newHolderValue = EditorGUI.Vector2Field(position, this.label, newHolderValue);

			x.intValue = Mathf.Clamp(Mathf.RoundToInt(newHolderValue.x), int.MinValue, int.MaxValue);
			y.intValue = Mathf.Clamp(Mathf.RoundToInt(newHolderValue.y), int.MinValue, int.MaxValue);
		}
	}
}