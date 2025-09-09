using UnityEngine;
using UnityEngine.XR;

public class VRPauseController : MonoBehaviour
{
    [Header("VR Settings")]
    public XRNode controllerNode = XRNode.RightHand;
    public VRMenuManager menuManager;
    
    private bool wasMenuButtonPressed = false;

    void Update()
    {
        // VR Controller menü butonu kontrolü
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        
        if (device.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButton))
        {
            if (menuButton && !wasMenuButtonPressed)
            {
                if (menuManager.isPaused)
                    menuManager.ResumeGame();
                else
                    menuManager.PauseGame();
            }
            wasMenuButtonPressed = menuButton;
        }
    }
}