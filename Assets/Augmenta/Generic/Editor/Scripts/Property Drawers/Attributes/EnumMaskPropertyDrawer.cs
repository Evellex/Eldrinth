using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	[CustomPropertyDrawer(typeof(EnumMask))]
	public class EnumFlagsAttributeDrawer : PropertyDrawer
	{
		private GUIContent label = new GUIContent();

		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			EditorGUIExt.BeginChangeCheckMulti(_property);
			label.text = _label.text;
			label.tooltip = _label.tooltip;
			int propertyValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
			if (EditorGUIExt.EndChangeCheckMulti())
				_property.intValue = propertyValue;
		}
	}
}