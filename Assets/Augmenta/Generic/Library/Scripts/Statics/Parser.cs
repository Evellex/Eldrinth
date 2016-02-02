using System.Collections;
using UnityEngine;

namespace Augmenta
{
	public static class Parser
	{
		public static bool TryParseBoolean(string input, out bool output)
		{
			input = input.ToLower();
			if (input == "true" || input == "t" || input == "yes" || input == "y")
			{
				output = true;
				return true;
			}
			if (input == "false" || input == "f" || input == "no" || input == "n")
			{
				output = false;
				return true;
			}
			int intValue;
			bool intSuccess = int.TryParse(input, out intValue);
			if (intSuccess)
			{
				if (intValue == 1)
				{
					output = true;
					return true;
				}
				if (intValue == 0)
				{
					output = false;
					return true;
				}
			}
			output = false;
			return false;
		}
	}
}