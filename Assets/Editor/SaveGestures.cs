using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class SaveGestures : Editor
{
    public override void OnInspectorGUI()
    {

        Player player = (Player) target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (player.isSavingGestures)
        {
            if (GUILayout.Button("Start saving gestures (ERASE ALL)"))
            {
                player.startSavingGestures();
            }

            if (GUILayout.Button("Add gesture"))
            {
                player.addGesture();
            }
        }
    }

    
}

