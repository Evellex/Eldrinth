using UnityEngine;
using System.Collections;

namespace Augmenta
{
	[ExecuteInEditMode]
	[AddComponentMenu("Augmenta/Colour Temperature Action")]
	public class ColourTemperature : MonoBehaviour
	{
		[SerializeField]
		[Range(0, 10000)]
		float colourTemperature = 6500;

		[SerializeField]
		ColorEvent onUpdate;

		void Update()
		{
			onUpdate.Invoke(ColorExt.ColourTemperature(colourTemperature));			
		}

		void OnValidate()
		{
			colourTemperature = Mathf.Clamp(colourTemperature, 0, 10000);
		}
	}
}
