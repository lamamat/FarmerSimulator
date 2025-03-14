using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Time_Manager))]
public class Time_Manager_Editor : Editor {
    public override void OnInspectorGUI() {
        Time_Manager timeManager = (Time_Manager)target;

        // Display runtime information only if the game is playing
        if (Application.isPlaying) {
            EditorGUILayout.LabelField("Runtime Information", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Current Time Stage: {timeManager.currentTimeStage}");
            EditorGUILayout.LabelField($"Time Elapsed (seconds): {timeManager.GetCurrentTimeElapsed()}");
        }

        // Draw the default Inspector GUI
        base.OnInspectorGUI();
    }
}

