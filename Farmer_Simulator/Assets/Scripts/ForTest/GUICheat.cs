using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUICheat : MonoBehaviour
{

     void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "To Next Day / Night"))
            Time_Manager.instance.SwitchTimeStage();
        if (GUI.Button(new Rect(10, 55, 100, 50), "To Next Day"))
            Time_Manager.instance.ToNextDay();
    }
}
