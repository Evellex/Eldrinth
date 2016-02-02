using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour
{
    public bool triggerPress = false;
    public bool triggerState = false;
    public bool mainButtonPressed = false;
    public bool secondaryButtonPressed = false;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    public bool hasPointer;
    public Vector2 touchpadVector;

    // Update is called once per frame
    void Update()
    {

        if (!trackedObj) trackedObj = GetComponent<SteamVR_TrackedObject>();

        if (trackedObj != null)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);
        }

        if (device != null)
        {
            triggerPress = device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger);
            triggerState = device.GetTouch(SteamVR_Controller.ButtonMask.Trigger);
            mainButtonPressed = device.GetTouch(SteamVR_Controller.ButtonMask.ApplicationMenu);
            secondaryButtonPressed = device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad);
            touchpadVector = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

        }
    }

    public void Vibrate()
    {

        device.TriggerHapticPulse(1000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
    }
}
