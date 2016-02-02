using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	public static class ModifyConditionRegister_MenuItem
	{
		private static ConditionRegister registerInstance;

		[MenuItem("Augmenta/Modifiy Condition Register")]
		public static void SelectInstance()
		{
			string assetsPath = Application.dataPath;
			string folderPath = "Augmenta/Generic/Library/Data/Resources/Augmenta";
			string filename = "Condition Register.asset";

			registerInstance = AssetDatabase.LoadAssetAtPath<ConditionRegister>("Assets/" + folderPath + "/" + filename);
			if (registerInstance == null)
			{
				if (!System.IO.Directory.Exists(assetsPath + "/" + folderPath))
					System.IO.Directory.CreateDirectory(assetsPath + "/" + folderPath);
				registerInstance = ScriptableObject.CreateInstance<ConditionRegister>();
				AssetDatabase.CreateAsset(registerInstance, "Assets/" + folderPath + "/" + filename);
			}
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = registerInstance;
			EditorGUIUtility.PingObject(registerInstance);
		}
	}
}