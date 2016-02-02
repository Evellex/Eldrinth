using UnityEngine;
using System.Collections;
using System.IO;

namespace Augmenta
{
    [ExecuteInEditMode]
    [AddComponentMenu("Augmenta/Screenshot Taker")]
    public class ScreenshotTaker : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 4)]
        int superSize = 1;
        
        public void TakeScreenshot()
        {
            Screenshot.TakeScreenshot(superSize);
        }
    }
}
