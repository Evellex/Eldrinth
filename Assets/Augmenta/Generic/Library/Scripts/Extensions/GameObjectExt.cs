using System.Linq;
using UnityEngine;

namespace Augmenta
{
	public static class GameObjectExt
	{
		public static T[] GetInterfaces<T>(this GameObject comp) where T : class
		{
			if (!typeof(T).IsInterface)
			{
				Console.PrintError(typeof(T).ToString() + ": is not of the interface type!");
				return Enumerable.Empty<T>() as T[];
			}
			return comp.GetComponents<Component>().OfType<T>().ToArray();
		}

		public static T[] GetInterfacesInChildren<T>(this GameObject comp, bool includeInactive = false) where T : class
		{
			if (!typeof(T).IsInterface)
			{
				Console.PrintError(typeof(T).ToString() + ": is not of the interface type!");
				return Enumerable.Empty<T>() as T[];
			}
			return comp.GetComponentsInChildren<Component>(includeInactive).OfType<T>().ToArray();
		}

		public static T GetOrAddComponent<T>(this GameObject target) where T : Component
		{
			T newComponent = target.GetComponent<T>();
			if (newComponent == null)
				return target.AddComponent<T>();
			else
				return newComponent;
		}
	}
}