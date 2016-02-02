using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	public static class GraphicsQuality
	{
		[RuntimeInitializeOnLoadMethod]
		private static void Initialise()
		{
			Console.Command newCommand;
			List<Console.Command.Argument> newArguments;

			newArguments = new List<Console.Command.Argument>();
			newArguments.Add(new Console.Command.Argument("factor", "The downscale factor for textures (0-3)", true));
			newCommand = new Console.Command("gfx_texture_downscale", "Determines the quality of the game's textures. Higher values mean smaller textures.", newArguments, TextureQualityCommand, TextureQualityValueReturn);
			Console.RegisterCommand(newCommand);
		}

		private static void TextureQualityCommand(string[] args)
		{
			if (args.Length > 0)
			{
				int newValue;
				bool parsed = int.TryParse(args[0], out newValue);
				if (parsed)
				{
					newValue = Mathf.Clamp(newValue, 0, 3);
					QualitySettings.masterTextureLimit = newValue;
				}
				else
					Console.PrintError("\"" + args[0] + "\" is not a valid integer!");
			}
			else
				Console.PrintError("Incorrect argument count for texture_quality command!");
		}

		private static string TextureQualityValueReturn()
		{
			return QualitySettings.masterTextureLimit.ToString();
		}
	}
}