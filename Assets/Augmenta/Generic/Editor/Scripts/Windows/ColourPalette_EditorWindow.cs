using Augmenta;
using UnityEditor;
using UnityEngine;

namespace AugmentaEditor
{
	public class ColourPalette_EditorWindow : EditorWindow
	{
		private const int textureSize = 512;
		private const int textureResolution = 512;
		private const int maxValue = textureSize - 1;
		private const int altSliders = 4;
		private static GUIStyle pickerBox;
		private static GUIStyle pickerCircle;
		private static GUIStyle sliderVertArrow;
		private static GUIStyle sliderHorizArrow;
		private static bool stylesInitialised = false;
		private Colour currentColourXYZ;
		private Color currentColourRGB;

		private Texture2D pickerTexture = null;
		private Texture2D sliderTexture = null;
		private Texture2D[] sliderGroupTexture = null;

		private Color[] pickerPixels;
		private Color[] sliderPixels;

		private Color[][] sliderGroupPixels;

		private Int4 pickerCoords = new Int4(textureSize, textureSize, textureSize, textureSize);

		private bool pickerOutOfDate = false;
		private bool sliderOutOfDate = false;

		private Rect pickerRect;
		private Rect sliderRect;
		private Rect[] sliderGroupRect = new Rect[altSliders];

		private Colour.Model colourModel = Colour.Model.RGB;
		private Colour.RGBSpace workingSpace = Colour.RGBSpace.sRGB;
		private Colour.RGBSpace limitingSpace = Colour.RGBSpace.sRGB;
		private SliderAxis sliderAxis = SliderAxis.Z;
		private SelectionMode selectionMode = SelectionMode.None;

		private enum SliderAxis
		{
			X,
			Y,
			Z,
		}

		private enum LabelLocation
		{
			Bottom,
			Left,
			Right,
		}

		private enum SelectionMode
		{
			SliderX = 0,
			SliderY = 1,
			SliderZ = 2,
			SliderW = 3,

			None = 10,
			Picker = 11,
			Slider = 12,
		}

		[MenuItem("Augmenta/Windows/Colour Palette")]
		public static void ShowWindow()
		{
			GetWindow<ColourPalette_EditorWindow>(true, "Colour Palette", true);
		}

		private static void PrepareStyles()
		{
			if (stylesInitialised == false)
			{
				stylesInitialised = true;
				pickerBox = "ColorPickerBox";
				pickerCircle = "ColorPicker2DThumb";
				sliderVertArrow = "ColorPickerVertThumb";
				sliderHorizArrow = "ColorPickerHorizThumb";
			}
		}

		private static void DrawLabelOutsideRect(Rect position, string label, LabelLocation labelLocation)
		{
			Matrix4x4 matrix = GUI.matrix;
			Rect position2 = new Rect(position.x, position.y - 18f, position.width, 16f);
			switch (labelLocation)
			{
				case LabelLocation.Bottom:
					position2 = new Rect(position.x, position.yMax, position.width, 16f);
					break;

				case LabelLocation.Left:
					GUIUtility.RotateAroundPivot(-90f, position.center);
					break;

				case LabelLocation.Right:
					GUIUtility.RotateAroundPivot(90f, position.center);
					break;
			}
			EditorGUI.BeginDisabledGroup(true);
			GUI.Label(position2, label);
			EditorGUI.EndDisabledGroup();
			GUI.matrix = matrix;
		}

		private static Colour GetWorkingSpaceColour(Colour.Model originalModel, Colour.RGBSpace workingSpace, Double4 arrangement)
		{
			Colour workingSpaceColour = new Colour(Double3.zero);
			if (originalModel == Colour.Model.RGB)
				workingSpaceColour = Colour.From_RGB((Double3)arrangement, workingSpace);
			else if (originalModel == Colour.Model.xyY)
				workingSpaceColour = Colour.From_xyY((Double3)arrangement);
			else if (originalModel == Colour.Model.HSL)
				workingSpaceColour = Colour.From_HSL((Double3)arrangement, workingSpace);
			else if (originalModel == Colour.Model.HSV)
				workingSpaceColour = Colour.From_HSV((Double3)arrangement, workingSpace);
			else if (originalModel == Colour.Model.XYZ)
				workingSpaceColour = new Colour((Double3)arrangement);
			else if (originalModel == Colour.Model.Lab)
				workingSpaceColour = Colour.From_Lab((Double3)arrangement);
			//else if (originalModel == Colour.Model.LCH)
			//workingSpaceColour = Colour.From_LCH((Double3)arrangement);
			return workingSpaceColour;
		}

		private static Double3 GetPickerCoords(Colour col, Colour.Model newModel, Colour.RGBSpace workingSpace)
		{
			Double3 coords = Double3.zero;
			if (newModel == Colour.Model.RGB)
				coords = Colour.To_RGB(col, workingSpace);
			else if (newModel == Colour.Model.xyY)
				coords = Colour.To_xyY(col);
			else if (newModel == Colour.Model.HSL)
				coords = Colour.To_HSL(col, workingSpace);
			else if (newModel == Colour.Model.HSV)
				coords = Colour.To_HSV(col, workingSpace);
			else if (newModel == Colour.Model.XYZ)
				coords = new Double3(col.X, col.Y, col.Z);
			else if (newModel == Colour.Model.Lab)
				coords = Colour.To_Lab(col);
			//else if (originalModel == Colour.Model.LCH)
			//workingSpaceColour = Colour.From_LCH((Double3)arrangement);
			return coords;
		}

		private void OnEnable()
		{
			pickerPixels = new Color[textureResolution * textureResolution];
			pickerTexture = new Texture2D(textureResolution, textureResolution, TextureFormat.RGB24, false);
			pickerTexture.wrapMode = TextureWrapMode.Clamp;
			pickerRect = new Rect(10, 120, textureSize, textureSize);

			sliderPixels = new Color[textureResolution];
			sliderTexture = new Texture2D(1, textureResolution, TextureFormat.RGB24, false);
			sliderTexture.wrapMode = TextureWrapMode.Clamp;
			sliderRect = new Rect(textureSize + 20, 120, 30, textureSize);

			sliderGroupPixels = new Color[altSliders][];
			sliderGroupTexture = new Texture2D[altSliders];
			for (int i = 0; i < altSliders; ++i)
			{
				sliderGroupPixels[i] = new Color[textureResolution];
				sliderGroupTexture[i] = new Texture2D(textureResolution, 1, TextureFormat.RGB24, false);
				sliderGroupTexture[i].wrapMode = TextureWrapMode.Clamp;
				sliderGroupRect[i] = new Rect(10, 132 + textureSize + (32 * i), textureSize, 20);
			}

			for (int y = 0; y < textureResolution; ++y)
			{
				int i = altSliders - 1;
				float value = ((float)y) / textureResolution;
				sliderGroupPixels[i][y] = new Color(value, value, value, 1);
				sliderGroupTexture[i].SetPixels(sliderGroupPixels[i]);
				sliderGroupTexture[i].Apply();
			}

			pickerOutOfDate = sliderOutOfDate = true;

			pickerRect = new Rect(10, 120, textureSize, textureSize);
			sliderRect = new Rect(textureSize + 20, 120, 30, textureSize);

			titleContent.text = "Colour Palette";
			titleContent.tooltip = "Augmenta Colour Palette";
		}

		private void UpdatePickerCoordinates()
		{
			Int2 mousePosition = new Int2();

			if (Event.current.type == EventType.mouseDown)
			{
				if (pickerRect.Contains(Event.current.mousePosition))
				{
					sliderOutOfDate = true;
					selectionMode = SelectionMode.Picker;
				}
				if (sliderRect.Contains(Event.current.mousePosition))
				{
					pickerOutOfDate = true;
					selectionMode = SelectionMode.Slider;
				}
				if (sliderRect.Contains(Event.current.mousePosition))
				{
					pickerOutOfDate = true;
					selectionMode = SelectionMode.Slider;
				}
				for (int i = 0; i < altSliders; ++i)
				{
					if (sliderGroupRect[i].Contains(Event.current.mousePosition))
					{
						if (i < 3)
						{
							sliderOutOfDate = true;
							if (i == (int)sliderAxis)
								pickerOutOfDate = true;
						}
						selectionMode = (SelectionMode)i;
					}
				}
			}

			if ((Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseDrag) && selectionMode != SelectionMode.None)
			{
				mousePosition = (Int2)Event.current.mousePosition;
				if (selectionMode == SelectionMode.Picker)
				{
					mousePosition -= new Int2((int)pickerRect.xMin, (int)pickerRect.yMin);
					pickerCoords.x = mousePosition.x;
					pickerCoords.y = (textureSize - 1) - mousePosition.y;
					sliderOutOfDate = true;
				}
				else if (selectionMode == SelectionMode.Slider)
				{
					mousePosition -= new Int2((int)sliderRect.xMin, (int)sliderRect.yMin);
					pickerCoords.z = textureSize - mousePosition.y;
					pickerOutOfDate = true;
				}
				else
				{
					int sliderIndex = ((int)selectionMode);
					mousePosition -= new Int2((int)sliderGroupRect[sliderIndex].xMin, (int)sliderGroupRect[sliderIndex].yMin);
					pickerCoords[sliderIndex] = mousePosition.x;
					if (sliderIndex < 3)
					{
						sliderOutOfDate = true;
						if (sliderIndex == (int)sliderAxis)
							pickerOutOfDate = true;
					}
				}
				pickerCoords = MathfExt.Clamp(pickerCoords, 0, textureSize - 1);
			}

			if (Event.current.type == EventType.mouseUp)
				selectionMode = SelectionMode.None;
		}

		private void OnGUI()
		{
			PrepareStyles();
			UpdatePickerCoordinates();

			EditorGUI.BeginChangeCheck();
			EditorGUI.BeginChangeCheck();
			colourModel = (Colour.Model)EditorGUILayout.Popup("Colour Space", (int)colourModel, System.Enum.GetNames(typeof(Colour.Model)));
			sliderAxis = (SliderAxis)EditorGUILayout.Popup("Slider Axis", (int)sliderAxis, System.Enum.GetNames(typeof(SliderAxis)));
			workingSpace = (Colour.RGBSpace)EditorGUILayout.Popup("Working Colour Space", (int)workingSpace, System.Enum.GetNames(typeof(Colour.RGBSpace)));
			if (EditorGUI.EndChangeCheck())
			{
				Double3 newPickFrac = GetPickerCoords(currentColourXYZ, colourModel, workingSpace);
				Double3 pick = newPickFrac * maxValue;
				pickerCoords = new Int4(Mathf.RoundToInt((float)pick.x), Mathf.RoundToInt((float)pick.y), Mathf.RoundToInt((float)pick.z), pickerCoords.w);
			}
			limitingSpace = (Colour.RGBSpace)EditorGUILayout.Popup("Limiting Colour Space", (int)limitingSpace, System.Enum.GetNames(typeof(Colour.RGBSpace)));

			if (EditorGUI.EndChangeCheck())
			{
				pickerOutOfDate = sliderOutOfDate = true;
			}

			EditorGUILayout.Space();
			EditorGUILayout.ColorField(currentColourRGB);

			minSize = maxSize = new Vector2(textureSize + 60, textureSize + 280);

			UpdateTextures();

			GUI.DrawTexture(pickerRect, pickerTexture, ScaleMode.StretchToFill);
			GUI.DrawTexture(sliderRect, sliderTexture, ScaleMode.StretchToFill);
			for (int i = 0; i < altSliders; ++i)
				GUI.DrawTexture(sliderGroupRect[i], sliderGroupTexture[i], ScaleMode.StretchToFill);

			int circleSize = 8;
			int halfCircleSize = 4;

			if (Event.current.type == EventType.Repaint)
			{
				pickerBox.Draw(pickerRect, GUIContent.none, 0);
				pickerBox.Draw(sliderRect, GUIContent.none, 0);
				pickerCircle.Draw(new Rect(pickerCoords.x + 10 - halfCircleSize, ((textureSize - 1) - (pickerCoords.y)) + 120 - halfCircleSize, circleSize, circleSize), GUIContent.none, 0);
				sliderVertArrow.Draw(new Rect(textureSize + 18, (textureSize - 1) - (pickerCoords.z) + 117, 34, 8), GUIContent.none, 0);
				for (int i = 0; i < altSliders; ++i)
				{
					sliderHorizArrow.Draw(new Rect(pickerCoords[i] + 7, sliderGroupRect[i].yMin - 4, 16, 28), GUIContent.none, 0);
					pickerBox.Draw(sliderGroupRect[i], GUIContent.none, 0);
				}
			}

			Double4 pickerPosFraction = pickerCoords / maxValue;

			if (pickerOutOfDate == true || sliderOutOfDate == true)
			{
				Double4 arrangement = GetPickerArrangement(sliderAxis, pickerPosFraction);
				Colour pickedColour = GetWorkingSpaceColour(colourModel, workingSpace, arrangement);

				if (Colour.IsInsideGamut(pickedColour, limitingSpace))
				{
					currentColourXYZ = pickedColour;
					currentColourRGB = (Color)Colour.To_RGB(pickedColour, workingSpace);
				}
				else
				{
					currentColourXYZ = new Colour(Double3.zero);
					currentColourRGB = Color.black;
				}
			}

			currentColourRGB.a = (float)pickerPosFraction.w;

			Repaint();

			pickerOutOfDate = sliderOutOfDate = false;
		}

		private void UpdateTextures()
		{
			if (pickerOutOfDate || sliderOutOfDate)
			{
				Double4 pickerPosFraction = pickerCoords / maxValue;
				Double4 arrangement = new Double4();
				Colour pickedColour;

				for (int y = 0; y < textureResolution; ++y)
				{
					if (pickerOutOfDate)
					{
						double yFraction = ((double)y / (double)maxValue);
						int yy = (y * textureResolution);
						for (int x = 0; x < textureResolution; ++x)
						{
							double xFraction = ((double)x / (double)maxValue);
							arrangement = GetPickerArrangement(sliderAxis, xFraction, yFraction, pickerPosFraction.z, pickerPosFraction.w);
							pickedColour = GetWorkingSpaceColour(colourModel, workingSpace, arrangement);
							if (Colour.IsInsideGamut(pickedColour, limitingSpace))
								pickerPixels[yy + x] = (Color)Colour.To_RGB(pickedColour, workingSpace);
							else
								pickerPixels[yy + x] = Color.black;
						}
					}
					if (sliderOutOfDate)
					{
						arrangement = GetPickerArrangement(sliderAxis, pickerPosFraction.x, pickerPosFraction.y, ((float)y) / textureResolution, pickerPosFraction.w);
						pickedColour = GetWorkingSpaceColour(colourModel, workingSpace, arrangement);
						if (Colour.IsInsideGamut(pickedColour, limitingSpace))
							sliderPixels[y] = (Color)Colour.To_RGB(pickedColour, workingSpace);
						else
							sliderPixels[y] = Color.black;
					}

					for (int i = 0; i < altSliders - 1; ++i)
					{
						float value = ((float)y) / textureResolution;
						arrangement = new Double4(pickerPosFraction.x, pickerPosFraction.y, pickerPosFraction.z, pickerPosFraction.w);
						arrangement[i] = value;
						pickedColour = GetWorkingSpaceColour(colourModel, workingSpace, arrangement);
						if (Colour.IsInsideGamut(pickedColour, limitingSpace))
							sliderGroupPixels[i][y] = (Color)Colour.To_RGB(pickedColour, workingSpace);
						else
							sliderGroupPixels[i][y] = Color.black;
					}
				}
				if (pickerOutOfDate)
				{
					pickerTexture.SetPixels(pickerPixels);
					pickerTexture.Apply(false);
				}
				if (sliderOutOfDate)
				{
					sliderTexture.SetPixels(sliderPixels);
					sliderTexture.Apply(false);
				}
				for (int i = 0; i < altSliders - 1; ++i)
				{
					sliderGroupTexture[i].SetPixels(sliderGroupPixels[i]);
					sliderGroupTexture[i].Apply();
				}
			}
		}

		private Double4 GetPickerArrangement(SliderAxis currentAxis, double x, double y, double z, double w)
		{
			Double4 arrangement = new Double4();
			arrangement.w = w;
			switch (currentAxis)
			{
				case SliderAxis.X:
					arrangement.x = z;
					arrangement.y = x;
					arrangement.z = y;
					break;

				case SliderAxis.Y:
					arrangement.x = x;
					arrangement.y = z;
					arrangement.z = y;
					break;

				case SliderAxis.Z:
					arrangement.x = x;
					arrangement.y = y;
					arrangement.z = z;
					break;
			}
			return arrangement;
		}

		private Double4 GetPickerArrangement(SliderAxis currentAxis, Double4 v)
		{
			return GetPickerArrangement(currentAxis, v.x, v.y, v.z, v.w);
		}
	}
}