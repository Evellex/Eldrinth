using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Augmenta
{
	public static class Console
	{
		private static ConsoleObject consoleInstance = null;
		private static bool active = false;
		private static GameObject oldInputSelection = null;
		private static StringBuilder textLog = new StringBuilder();

		private static Dictionary<string, Command> commands = new Dictionary<string, Command>();

		private static List<string> submittedCommands = new List<string>();
		private static List<string> predictedCommands = new List<string>();
		private static List<string> autoCompleteCommands = new List<string>();

		private static int commandSelectionIndex = 0;

		private static int debugInfoLevel = 1;

		private static InputState inputState = InputState.User;

		private static string commandPromptString = "Enter command...";
		private static string noEventSystemErrorString = "<color=red>An EventSystem needs to be present in one of the active scenes in order to be able to input commands into the console.</color>";

		private enum InputState
		{
			User,
			History,
			Prediction,
			AutoComplete,
		}

		public static int DebugInfoLevel
		{
			get { return DebugInfoLevel; }
		}

		public static bool IsActive
		{
			get { return active; }
		}

		public static void RegisterCommand(Command newCommand)
		{
			commands.Add(newCommand.identifier, newCommand);
		}

		public static void PrintText(object newText, bool duplicateToUnity = false)
		{
			if (newText == null)
				newText = "Null";
			if ((Debug.isDebugBuild || Application.isEditor) && duplicateToUnity)
				Debug.Log("Augmenta Console: " + newText);
			if (Application.isPlaying)
			{
				string nextText = Environment.NewLine + newText;
				while (textLog.Length + nextText.Length > 10000)
					textLog.Remove(0, textLog.ToString().IndexOf(Environment.NewLine) + 1);
				textLog.Append(nextText);
				consoleInstance.Log.text = textLog.ToString();
			}
		}

		public static void PrintWarning(object newText, bool duplicateToUnity = false)
		{
			if ((Debug.isDebugBuild || Application.isEditor) && duplicateToUnity)
				Debug.LogWarning("Augmenta Console: " + newText);
			PrintText("<color=orange>" + newText + "</color>");
		}

		public static void PrintError(object newText, bool duplicateToUnity = true)
		{
			if ((Debug.isDebugBuild || Application.isEditor) && duplicateToUnity)
				Debug.LogError("Augmenta Console: " + newText);
			PrintText("<color=red>" + newText + "</color>");
		}

		public static void CallCommand(CommandCall commandCall)
		{
			if (commands.ContainsKey(commandCall.identifier))
			{
				Command targetCommand = commands[commandCall.identifier];
				if (targetCommand.minimumArguments > 0 && commandCall.args.Length == 0)
					PrintHelpForCommand(targetCommand);
				else if (targetCommand.minimumArguments > commandCall.args.Length)
					PrintWarning("Command \"" + commandCall.identifier + "\" requires " + targetCommand.minimumArguments + " arguments!");
				else
					commands[commandCall.identifier].executeFunction(commandCall.args);
			}
			else
				PrintWarning("Command \"" + commandCall.identifier + "\" not found!");
		}

		public static void ChangeDebugInfoLevel(int level)
		{
			debugInfoLevel = Mathf.Clamp(level, 0, 3);
		}

		private static void ChangeDebugInfoLevelCommand(string[] args)
		{
			int value;
			bool valid = int.TryParse(args[0], out value);
			if (valid)
				ChangeDebugInfoLevel(value);
			else
				PrintWarning("\"" + args[0] + "\" is not a valid integer!");
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Initialise()
		{
			GameObject consolePrefab = Resources.Load("Console Prefab") as GameObject;
			consoleInstance = ObjectPooler.Withdraw(consolePrefab, Vector3.zero, Quaternion.identity).GetComponent<ConsoleObject>();
			consoleInstance.gameObject.name = "Console";
			GameObject.DontDestroyOnLoad(consoleInstance.gameObject);

			consoleInstance.Log.text = "";
			consoleInstance.hideFlags = HideFlags.NotEditable;
			consoleInstance.InputField.onValueChange.AddListener(OnInputChange);
			RegisterInbuiltCommands();

			Application.logMessageReceived += UnityLogMessageReceived;
			Events.On_Update += Update;
		}

		private static void UnityLogMessageReceived(string condition, string stackTrace, LogType type)
		{
			if (debugInfoLevel > 0)
			{
				string text = "";
				bool error = type == LogType.Assert || type == LogType.Error || type == LogType.Exception;
				if (debugInfoLevel == 3 || (debugInfoLevel == 2 && error))
					text = condition + Environment.NewLine + stackTrace;
				else
					text = condition;
				if (type == LogType.Log)
					PrintText(text, false);
				if (type == LogType.Warning)
					PrintWarning(text, false);
				if (type == LogType.Error)
					PrintError(text, false);
				if (type == LogType.Exception)
					PrintError(text, false);
				if (type == LogType.Assert)
					PrintError(text, false);
			}
		}

		private static void RegisterInbuiltCommands()
		{
			Command newCommand;
			List<Command.Argument> newArguments;

			newArguments = new List<Command.Argument>(1);
			newArguments.Add(new Command.Argument("search string", "The string to search for in command names.", true));
			newCommand = new Command("find", "Find console commands containing the search term.", newArguments, FindCommand);
			RegisterCommand(newCommand);

			newArguments = new List<Command.Argument>(1);
			newArguments.Add(new Command.Argument("target command", "The command to get information on.", true));
			newCommand = new Command("help", "Get information on a console command.", newArguments, HelpCommand);
			RegisterCommand(newCommand);

			newArguments = new List<Command.Argument>(1);
			newArguments.Add(new Command.Argument("text", "The text to print to the console.", true));
			newCommand = new Command("echo", "Prints text to the console.", newArguments, PrintTextCommand);
			RegisterCommand(newCommand);

			newArguments = new List<Command.Argument>(1);
			newArguments.Add(new Command.Argument("level", "The level of information (0 = none, 1 = title, 2 = stack trace (errors only), 3 = stack trace (all)", true));
			newCommand = new Command("debug_info_level", "Determines the amount of information printed on an internal debug call.", newArguments, ChangeDebugInfoLevelCommand, () => { return debugInfoLevel.ToString(); });
			RegisterCommand(newCommand);

			newArguments = new List<Command.Argument>(0);
			newCommand = new Command("close", "Closes the console.", newArguments, CloseConsoleCommand);
			RegisterCommand(newCommand);

			newCommand = new Command("clear", "Clears the console.", newArguments, ClearConsoleCommand);
			RegisterCommand(newCommand);

			newCommand = new Command("quit", "Closes the application.", newArguments, CloseApplicationCommand);
			RegisterCommand(newCommand);
		}

		private static void OnInputChange(string input)
		{
			if (inputState != InputState.User)
			{
				bool match = false;
				match |= predictedCommands.Contains(input);
				match |= submittedCommands.Contains(input);
				if (!match)
					inputState = InputState.User;
			}
			if (inputState == InputState.User)
			{
				commandSelectionIndex = 0;
				RebuildPredictedCommands(input);
				RebuildAutoCompleteCommands(input);
				RebuildPredictionBox();
			}
			else if (inputState == InputState.History)
			{
				autoCompleteCommands.Clear();
				predictedCommands.Clear();
				RebuildPredictionBox();
			}
		}

		private static void RebuildAutoCompleteCommands(string input)
		{
			autoCompleteCommands.Clear();
			if (input.Length > 0)
			{
				foreach (string key in commands.Keys)
				{
					if (key.StartsWith(input))
						autoCompleteCommands.Add(key);
				}
				autoCompleteCommands.Sort();
			}
		}

		private static void RebuildPredictedCommands(string input)
		{
			predictedCommands.Clear();
			if (input.Length > 0)
			{
				foreach (string key in commands.Keys)
				{
					if (key.Contains(input))
						predictedCommands.Add(key);
				}
				predictedCommands.Sort();
			}
		}

		private static void RebuildPredictionBox()
		{
			consoleInstance.PredictionText.text = "";
			string seperator = "  |  ";
			StringBuilder commandsString = new StringBuilder();
			for (int i = 0; i < predictedCommands.Count; ++i)
			{
				bool autoCompleteMatch = autoCompleteCommands.Contains(predictedCommands[i]);
				if (autoCompleteMatch)
					commandsString.Append("<color=lime>");
				commandsString.Append(predictedCommands[i]);
				if (autoCompleteMatch)
					commandsString.Append("</color>");
				if (i != predictedCommands.Count - 1)
					commandsString.Append(seperator);
			}
			consoleInstance.PredictionText.text = commandsString.ToString();
		}

		private static void HelpCommand(string[] args)
		{
			if (commands.ContainsKey(args[0]))
				PrintHelpForCommand(commands[args[0]]);
			else
				PrintWarning("Command \"" + args[0] + "\" not found!");
		}

		private static void PrintHelpForCommand(Command command)
		{
			StringBuilder commandLine = new StringBuilder();
			commandLine.Append(command.identifier);
			for (int i = 0; i < command.arguments.Count; ++i)
			{
				if (command.arguments[i].required)
					commandLine.Append(" <" + command.arguments[i].name + ">");
				else
					commandLine.Append(" [" + command.arguments[i].name + "]");
			}
			PrintText("<color=lime>" + commandLine.ToString() + "</color>");
			PrintText("\tDescription:");
			PrintText("\t\t" + command.description);
			if (command.arguments.Count > 0)
			{
				PrintText("\t\tArguments:");
				for (int i = 0; i < command.arguments.Count; ++i)
				{
					PrintText("\t\t" + command.arguments[i].name);
					PrintText("\t\t\t" + command.arguments[i].description);
				}
			}
			if (command.valueReturn != null)
			{
				PrintText("\tValue:");
				PrintText("\t\t" + command.valueReturn.Invoke());
			}
		}

		private static void FindCommand(string[] args)
		{
			bool anyExist = false;
			foreach (string key in commands.Keys)
			{
				if (key.Contains(args[0]))
				{
					anyExist = true;
					PrintHelpForCommand(commands[key]);
				}
			}
			if (!anyExist)
				PrintWarning("No command containing \"" + args[0] + "\" could be found!");
		}

		private static void PrintTextCommand(string[] args)
		{
			PrintText(args[0]);
		}

		private static void Update()
		{
			if (Input.GetKeyDown(KeyCode.BackQuote))
				ToggleConsole();
			if (active)
				UpdateConsole();
		}

		private static void UpdateConsole()
		{
			if (EventSystem.current != null)
			{
				consoleInstance.InputPrompt.text = commandPromptString;
				if (consoleInstance.PredictionText.text == noEventSystemErrorString)
					OnInputChange("");
			}
			else
			{
				consoleInstance.PredictionText.text = noEventSystemErrorString;
				consoleInstance.InputPrompt.text = "";
			}

			if (!consoleInstance.InputField.isFocused)
			{
				if (EventSystem.current != null)
					consoleInstance.InputField.Select();
				consoleInstance.InputField.MoveTextStart(true);
				consoleInstance.InputField.MoveTextEnd(false);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Tab))
			{
				if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
				{
					InputState targetState1 = InputState.History;
					InputState targetState2 = InputState.Prediction;
					List<string> targetList1 = submittedCommands;
					List<string> targetList2 = predictedCommands;

					if (Input.GetKeyDown(KeyCode.UpArrow))
					{
						targetState1 = InputState.History;
						targetState2 = InputState.Prediction;
						targetList1 = submittedCommands;
						targetList2 = predictedCommands;
					}
					else if (Input.GetKeyDown(KeyCode.DownArrow))
					{
						targetState1 = InputState.Prediction;
						targetState2 = InputState.History;
						targetList1 = predictedCommands;
						targetList2 = submittedCommands;
					}

					if (inputState == InputState.User || inputState == targetState1 || inputState == InputState.AutoComplete)
					{
						if (inputState != targetState1)
						{
							inputState = targetState1;
							commandSelectionIndex = 0;
						}
						else
							commandSelectionIndex = Mathf.Clamp(commandSelectionIndex + 1, 0, targetList1.Count - 1);
						if (targetList1.Count > 0)
							consoleInstance.InputField.text = targetList1[commandSelectionIndex];
						else
							consoleInstance.InputField.text = "";
					}
					if (inputState == targetState2)
					{
						if (commandSelectionIndex == 0)
						{
							inputState = targetState1;
							if (targetList1.Count > 0)
								consoleInstance.InputField.text = targetList1[commandSelectionIndex];
							else
								consoleInstance.InputField.text = "";
						}
						else
						{
							commandSelectionIndex = Mathf.Clamp(commandSelectionIndex - 1, 0, targetList2.Count - 1);
							if (targetList2.Count > 0)
								consoleInstance.InputField.text = targetList2[commandSelectionIndex];
							else
								consoleInstance.InputField.text = "";
						}
					}
				}
				else if (Input.GetKeyDown(KeyCode.Tab))
				{
					if (inputState != InputState.AutoComplete)
					{
						inputState = InputState.AutoComplete;
						commandSelectionIndex = 0;
					}
					else
					{
						if (commandSelectionIndex == autoCompleteCommands.Count - 1)
							commandSelectionIndex = 0;
						else
							++commandSelectionIndex;
					}
					if (autoCompleteCommands.Count > 0)
						consoleInstance.InputField.text = autoCompleteCommands[commandSelectionIndex];
					else
						consoleInstance.InputField.text = "";
				}

				consoleInstance.InputField.MoveTextEnd(false);
			}
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				SubmitCommand(consoleInstance.InputField.text);
				commandSelectionIndex = 0;
			}
		}

		private static void ClearConsoleCommand(string[] args)
		{
			textLog.Remove(0, textLog.Length);
			consoleInstance.Log.text = textLog.ToString();
		}

		private static void CloseApplicationCommand(string[] args)
		{
			Application.Quit();
		}

		private static void ToggleConsole()
		{
			active = !active;
			consoleInstance.gameObject.SetActive(active);
			if (active)
			{
				if (EventSystem.current != null)
				{
					oldInputSelection = EventSystem.current.currentSelectedGameObject;
					consoleInstance.InputField.Select();
					consoleInstance.InputField.text = "";
					consoleInstance.InputPrompt.text = commandPromptString;
				}
				else
				{
					consoleInstance.InputPrompt.text = "";
					consoleInstance.InputField.text = "";
					consoleInstance.PredictionText.text = noEventSystemErrorString;
				}
			}
			else
			{
				if (EventSystem.current != null)
					EventSystem.current.SetSelectedGameObject(oldInputSelection);
				oldInputSelection = null;
				commandSelectionIndex = 0;
			}
		}

		private static void CloseConsoleCommand(string[] args)
		{
			ToggleConsole();
		}

		private static string SanatiseString(string value)
		{
			string returnValue = value;
			returnValue = returnValue.Replace("\n", "");
			returnValue = returnValue.Replace("\r\n", "");
			returnValue = returnValue.Replace("\r", "");
			returnValue = returnValue.Replace("\t", "");
			return returnValue;
		}

		private static void SubmitCommand(string commandText)
		{
			commandSelectionIndex = 0;
			consoleInstance.InputField.ActivateInputField();
			if (commandText != "")
			{
				commandText = SanatiseString(commandText);
				consoleInstance.InputField.text = "";
				submittedCommands.Insert(0, commandText);
				PrintText("<color=cyan>" + commandText + "</color>");
				CallCommand(ParseCommand(commandText));
			}
		}

		private static CommandCall ParseCommand(string commandText)
		{
			List<string> stringSegments = new List<string>();
			int spaceIndex = -1;
			int quoteIndex = -1;
			bool withinQuote = false;
			string workingCopyText = commandText;
			while (workingCopyText.Length > 0)
			{
				int target = 0;
				spaceIndex = workingCopyText.IndexOf(" ");
				quoteIndex = workingCopyText.IndexOf("\"");
				if (spaceIndex == -1 && quoteIndex == -1)
				{
					stringSegments.Add(workingCopyText);
					workingCopyText = workingCopyText.Remove(0);
				}
				else
				{
					if (!withinQuote)
					{
						if (spaceIndex == -1)
							target = quoteIndex;
						else if (quoteIndex == -1)
							target = spaceIndex;
						else
							target = Mathf.Min(spaceIndex, quoteIndex);
						if (target == quoteIndex)
							withinQuote = true;
						if (target > 0)
							stringSegments.Add(workingCopyText.Substring(0, target));
						workingCopyText = workingCopyText.Remove(0, target + 1);
					}
					else
					{
						stringSegments.Add(workingCopyText.Substring(0, quoteIndex));
						workingCopyText = workingCopyText.Remove(0, quoteIndex + 1);
						withinQuote = false;
					}
				}
			}
			CommandCall returnCommandCall = new CommandCall();
			returnCommandCall.identifier = stringSegments[0];
			returnCommandCall.args = new string[stringSegments.Count - 1];
			for (int i = 1; i < stringSegments.Count; ++i)
			{
				returnCommandCall.args[i - 1] = stringSegments[i];
			}
			return returnCommandCall;
		}

		public class Command
		{
			public string identifier;
			public string description;
			public Action<string[]> executeFunction;
			public Func<string> valueReturn;
			public List<Argument> arguments;

			public Command(string identifier, string description, List<Argument> arguments, Action<string[]> executeFunction, Func<string> valueReturn = null)
			{
				this.identifier = identifier;
				this.description = description;
				this.arguments = arguments;
				this.executeFunction = executeFunction;
				this.valueReturn = valueReturn;
				int requiredArguments = 0;
				arguments.Sort();
				for (int i = 0; i < arguments.Count; ++i)
				{
					if (arguments[i].required)
						++requiredArguments;
					else
						break;
				}
				minimumArguments = requiredArguments;
			}

			private Command()
			{
				this.identifier = "";
				this.description = "";
				this.arguments = null;
				this.executeFunction = null;
			}

			public int minimumArguments { get; private set; }

			public class Argument : IComparable<Argument>
			{
				public string name;
				public string description;
				public bool required;

				public Argument(string name, string description, bool required)
				{
					this.name = name;
					this.description = description;
					this.required = required;
				}

				private Argument()
				{
					this.name = "";
					this.description = "";
					this.required = true;
				}

				int IComparable<Argument>.CompareTo(Argument other)
				{
					if (!other.required && required)
						return -1;
					else if (other.required && !required)
						return 1;
					else
						return name.CompareTo(other.name);
				}
			}
		}

		public class CommandCall
		{
			public string identifier;
			public string[] args;
		}
	}
}