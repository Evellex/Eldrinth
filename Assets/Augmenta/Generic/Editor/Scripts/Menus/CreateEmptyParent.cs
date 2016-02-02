using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	public static class CreateEmptyParent
	{
		[MenuItem("GameObject/Create Empty Parent %#&n", priority = 0)]
		public static void CreateNewParent()
		{
			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName("Create Empty Parent");
			GameObject newParent = new GameObject("GameObject");
			Undo.RegisterCreatedObjectUndo(newParent, "Create Game Object");
			if (Selection.activeGameObject != null)
			{
				newParent.name = Selection.activeGameObject.name + " Parent";
				newParent.transform.position = Selection.activeGameObject.transform.position;
				Undo.SetTransformParent(Selection.activeGameObject.transform, newParent.transform, "Set Transform Parent");
			}
			Selection.activeGameObject = newParent;
			EditorGUIUtility.PingObject(newParent);
			Undo.IncrementCurrentGroup();
		}
	}
}