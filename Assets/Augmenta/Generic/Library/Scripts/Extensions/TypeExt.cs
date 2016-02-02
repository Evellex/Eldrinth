using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Augmenta
{	
	public static class TypeExt
	{
		public static IEnumerable<MethodInfo> GetMethodsBySig(this Type type, BindingFlags flags, Type returnType, params Type[] parameterTypes)
		{
			return type.GetMethods(flags).Where((m) =>
			{
				if (m.ReturnType != returnType) return false;
				var parameters = m.GetParameters();
				if ((parameterTypes == null || parameterTypes.Length == 0))
					return parameters.Length == 0;
				if (parameters.Length != parameterTypes.Length)
					return false;
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					if (parameters[i].ParameterType != parameterTypes[i])
						return false;
				}
				return true;
			});
		}

		public static IEnumerable<MethodInfo> GetMethodsBySig(this Type type, BindingFlags flags, Type returnType)
		{
			return type.GetMethods(flags).Where((m) =>
			{
				if (m.ReturnType != returnType) return false;
				var parameters = m.GetParameters();
				if (parameters.Length != 0)
					return false;
				return true;
			});
		}
	}
}
