using UnityEngine;
using System.Collections;
using System.IO;

namespace Augmenta
{ 
	static public class Screenshot 
	{
		const string screenshotFolder = "screenshots";
		static string fullPath = "";

		static public void TakeScreenshot(int superSize)
		{
			System.DateTime dateTime = System.DateTime.Now;
			string timeString = dateTime.Year + "_" + dateTime.Month + "_" + dateTime.Day;
			timeString = timeString + "_" + dateTime.Hour + "_" + dateTime.Minute + "_" + dateTime.Second;
			timeString = timeString + "_" + dateTime.Millisecond;
			fullPath = System.Environment.CurrentDirectory + "/" + screenshotFolder;
			if (!Directory.Exists(fullPath))
				Directory.CreateDirectory(fullPath);
			Application.CaptureScreenshot("/" + screenshotFolder + "/" + timeString + ".png", superSize);
		}
	}
}
