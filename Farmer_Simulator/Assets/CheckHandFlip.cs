using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CheckHandFlip : MonoBehaviour
{
    [SerializeField] GameObject DisplayUi;
    public XRNode controllerNode = XRNode.LeftHand; // ปุ่ม X อยู่ฝั่งซ้าย

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        // ตรวจสอบการกดปุ่ม X (primaryButton)
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
        {
            DisplayUi.SetActive(isPressed); // โชว์ UI ตอนกดค้าง
        }
    }
}
