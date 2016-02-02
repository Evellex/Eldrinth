using UnityEngine;

namespace Augmenta
{
	public class EnumMask : PropertyAttribute
	{
		public string enumName;
 
		public EnumMask() {}

		public EnumMask(string name)
		{
			enumName = name;
		}
	}
}
