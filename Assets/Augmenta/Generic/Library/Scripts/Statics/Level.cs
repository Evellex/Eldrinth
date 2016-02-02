using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	public static class Level
	{
		[System.Obsolete("This event is deprecated. Please use UnityEvents.On_LevelWasLoaded.", false)]
		public static event System.Action OnLoadCalled;

		[System.Obsolete("This event is deprecated. Please use UnityEvents.On_LevelWasLoaded.", false)]
		public static event System.Action OnLoadFinished;

		[System.Obsolete("This function is deprecated. Please use Application.LoadLevel.", false)]
		public static void Load(string name)
		{
			if (OnLoadCalled != null)
				OnLoadCalled.Invoke();
			Application.LoadLevel(name);
			if (OnLoadFinished != null)
				OnLoadFinished.Invoke();
		}

		[System.Obsolete("This function is deprecated. Please use Application.LoadLevel.", false)]
		public static void Load(int index)
		{
			if (OnLoadCalled != null)
				OnLoadCalled.Invoke();
			Application.LoadLevel(index);
			if (OnLoadFinished != null)
				OnLoadFinished.Invoke();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Initialise()
		{
			List<Console.Command.Argument> newArguments = new List<Console.Command.Argument>();
			newArguments.Add(new Console.Command.Argument("level", "The index of the level to load (0-" + (Application.levelCount - 1) + ")", true));
			Console.Command newCommand = new Console.Command("load_level", "Load a level by index", newArguments, LoadCommand);
			Console.RegisterCommand(newCommand);
		}

		private static void LoadCommand(string[] args)
		{
			if (args.Length > 0)
			{
				int index;
				bool success = int.TryParse(args[0], out index);
				if (success)
				{
					if (Application.levelCount <= index)
						Console.PrintError("No level with the index " + index + " exists!");
					else
#pragma warning disable 0618
						Load(index);
#pragma warning restore 0618
				}
				else
				{
					Console.PrintError("\"" + args[0] + "\" is not a valid integer!");
				}
			}
		}
	}
}