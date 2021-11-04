using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]
public class HelpWindow : EditorWindow
{
    static HelpWindow window;
    private Texture2D logo = null;
    private string wikiURL = "https://github.com/TSGameDev/RegionRoaming/wiki/1---Introduction";

    static HelpWindow()
    {
        EditorApplication.delayCall += () =>
        {
            if (!SessionState.GetBool("ProjectOpened", false))
            {
                SessionState.SetBool("ProjectOpened", true);
                if(window == null)
                    window = CreateInstance<HelpWindow>();
                
                window.Show();
            }
        };
    }

    [MenuItem("Region Roaming/ Help Window", false, 1)]
    public static void ShowWindow()
    {
        if (window == null)
            window = CreateInstance<HelpWindow>();
        window.Show();
    }

    private void OnEnable()
    {
        logo = (Texture2D)Resources.Load("Logo", typeof(Texture2D));
    }

    private void OnDisable()
    {
        Resources.UnloadUnusedAssets();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(logo, GUILayout.MaxWidth(800f), GUILayout.MaxHeight(200f));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Hello, Welcome to Region Roaming!");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("If you Haven't Yet I would suggest you read through the README file located within the file you downloaded from GitHub. The same one with this tools Unity Package in.");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("If you prefer a more detailed run-though of the tool you can read through the Wiki on the GitHub repo you downloaded the tool from, or click the button at the bottom of this window.");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button(new GUIContent("WIKI", "Opens the Wiki for Region Roaming in a browser"), GUILayout.Width(200f), GUILayout.Height(30f)))
        {
            Application.OpenURL(wikiURL);
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("Close", "Closes this window"), GUILayout.Width(200f), GUILayout.Height(30f)))
        {
            window.Close();
        }

        GUILayout.EndHorizontal();
    }
}
