using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomPropertyDrawer(typeof(Int3))]
	public class Int3_Drawer : PropertyDrawer
	{
		private GUIContent label = new GUIContent();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty x = property.FindPropertyRelative("x");
			SerializedProperty y = property.FindPropertyRelative("y");
			SerializedProperty z = property.FindPropertyRelative("z");

			this.label.text = ObjectNames.NicifyVariableName(property.name);
			this.label.tooltip = property.tooltip;

			Vector3 newHolderValue = new Vector3(x.intValue, y.intValue, z.intValue);
			newHolderValue = EditorGUI.Vector3Field(position, this.label, newHolderValue);
			x.intValue = Mathf.RoundToInt(newHolderValue.x);
			y.intValue = Mathf.RoundToInt(newHolderValue.y);
			z.intValue = Mathf.RoundToInt(newHolderValue.z);
		}
	}
}