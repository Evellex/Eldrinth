using UnityEngine;
using UnityEngine.UI;

namespace Augmenta
{
	[AddComponentMenu("")]
	public class ConsoleObject : MonoBehaviour
	{
		[SerializeField]
		private InputField inputField;

		[SerializeField]
		private Text inputPrompt;

		[SerializeField]
		private Text predictionText;

		[SerializeField]
		private Text log;

		public InputField InputField
		{
			get { return inputField; }
		}

		public Text InputPrompt
		{
			get { return inputPrompt; }
		}

		public Text PredictionText
		{
			get { return predictionText; }
		}

		public Text Log
		{
			get { return log; }
		}
	}
}