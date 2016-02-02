using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Augmenta
{
	public static class ComponentExt
	{
		public static T[] GetInterfaces<T>(this Component comp) where T : class
		{
			return comp.gameObject.GetInterfaces<T>();
		}

		public static T[] GetInterfacesInChildren<T>(this Component comp, bool includeInactive = false) where T : class
		{
			return comp.gameObject.GetInterfacesInChildren<T>(includeInactive);
		}
	}
}