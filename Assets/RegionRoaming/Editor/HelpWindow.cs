using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HelpWindow : EditorWindow
{
    Texture2D Logo;

    [MenuItem("Region Roaming/Help Window", false, 1)]
    private static void Init()
    {
        HelpWindow window = (HelpWindow)EditorWindow.GetWindow(typeof(HelpWindow));
        window.Show();
    }

    private void OnEnable()
    {
        Logo = Resources.Load("12345") as Texture2D;
    }

    private void OnGUI()
    {
        GUILayout.Label(Logo);
    }
}
