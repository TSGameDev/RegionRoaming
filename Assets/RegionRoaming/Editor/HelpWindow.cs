using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]
public class HelpWindow : EditorWindow
{
    static HelpWindow window;
    private Texture2D logo = null;
    private string wikiURL = "https://github.com/TSGameDev/RegionRoaming/wiki/1---Introduction";
    private string triangleNETForkURL = "https://github.com/Geri-Borbas/Triangle.NET";
    private string twitterURL = "https://twitter.com/TSGame_Dev";
    GUIStyle titleText;

    float leftSectionX = 50f;
    float rightSectionX = 450f;

    float sectionSizeWidth = 400f;
    float sectionSizeHeight = 200f;

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
        #region Setting TitleText
        if (titleText == null)
        {
            titleText = new GUIStyle(GUI.skin.label);
            titleText.fontStyle = FontStyle.Bold;
            titleText.fontSize = 16;
        }
        #endregion

        #region Logo
        GUILayout.BeginArea(new Rect(50f, 0f, 800f, 200f));
        GUILayout.Label(logo, GUILayout.MaxWidth(800f), GUILayout.MaxHeight(200f));
        GUILayout.EndArea();
        #endregion

        GUILayout.Space(10);

        #region Introduction
        GUILayout.BeginArea(new Rect(leftSectionX, 200f, 800, 200));
        GUILayout.Label("Introduction", titleText);
        GUILayout.Label("Hello and Welcome to Region Roaming!");
        GUILayout.Label($"This is a verticle slice of a full tool for a college course. It produces a Vector3 within an area for AI destination. {Environment.NewLine}It DOES NOT create any AI behaviour or logic.");
        GUILayout.EndArea();
        #endregion

        GUILayout.BeginHorizontal();

        #region Getting Started Region
        GUILayout.BeginVertical();
        GUILayout.BeginArea(new Rect(leftSectionX, 300f, sectionSizeWidth, sectionSizeHeight));

        GUILayout.Label("Getting Started", titleText);

        GUILayout.Label($"Region Romaing doesn't contain much in the way of set up, simply {Environment.NewLine}follow the following steps:");

        GUILayout.Space(10);

        GUILayout.Label("Navigate to the Layers tab.");
        GUILayout.Label("Create a layer, number doesn't matter called Terrain or terrain.");
        GUILayout.Label("Set your floor or terrain to the newly created Terrain/terrain layer.");

        GUILayout.Space(10);

        GUILayout.Label("Visit the Wiki from the button below a detailed rundown");
        if (GUILayout.Button(new GUIContent("WIKI", "Opens the Wiki for Region Roaming in a browser"), GUILayout.Width(200f), GUILayout.Height(30f)))
        {
            Application.OpenURL(wikiURL);
        }

        GUILayout.EndArea();
        GUILayout.EndVertical();
        #endregion

        #region Sample Scenes Region
        GUILayout.BeginVertical();
        GUILayout.BeginArea(new Rect(rightSectionX, 300f, sectionSizeWidth, sectionSizeHeight));

        GUILayout.Label("Sample Scenes", titleText);

        GUILayout.Label($"The tool comes with 3 different demo scenes, if you haven't removed {Environment.NewLine}from when importing, that can be found at the following filepath:");

        GUILayout.Space(10);

        GUILayout.Label("RegionRoaming/Demo/Scenes");

        GUILayout.Space(10);

        GUILayout.Label($"The different scenes showcase the different functions. {Environment.NewLine}Read about each function on the Wiki or README.");

        GUILayout.EndArea();
        GUILayout.EndVertical();
        #endregion

        #region Advanced Topic

        GUILayout.BeginVertical();
        GUILayout.BeginArea(new Rect(leftSectionX, 500f, sectionSizeWidth, sectionSizeHeight + 50));

        GUILayout.Label("Advanced Topic", titleText);

        GUILayout.Label($"This tool comes with some small additions for more advanced users {Environment.NewLine}such as a mathematical script and the Triangle.NET libirary");

        GUILayout.Space(10);

        GUILayout.Label("RegionRoaming.Matematics");
        GUILayout.Label("Triangle.NET");

        GUILayout.Space(10);

        GUILayout.Label($"The functions for the mathmatics script are explained in depth {Environment.NewLine}on the Wiki");
        GUILayout.Label($"You can find a fork of the Triangle.NET library here. {Environment.NewLine}You dont need to download it the library is included in this tool");

        if (GUILayout.Button(new GUIContent("Triangle.NET", "Opens the fork Github for Triangle.NET"), GUILayout.Width(200f), GUILayout.Height(30f)))
        {
            Application.OpenURL(triangleNETForkURL);
        }

        GUILayout.EndArea();
        GUILayout.EndVertical();

        #endregion

        #region Documentation and Support

        GUILayout.BeginVertical();
        GUILayout.BeginArea(new Rect(rightSectionX, 500f, sectionSizeWidth, sectionSizeHeight + 80));

        GUILayout.Label("Documentation and Support", titleText);

        GUILayout.Label($"As mentioned throughout this Helpwindow, there are 2 locations of {Environment.NewLine}documentation:");
        
        GUILayout.Space(10);

        GUILayout.Label($"Wiki - The wiki is the best place to get a detailed rundown and have {Environment.NewLine}everything wihtin the tool explained.");

        GUILayout.Space(10);

        GUILayout.Label($"README - The README contains similar information to the wiki {Environment.NewLine}but isn't as easily navigated as the Wiki");

        GUILayout.Space(10);

        GUILayout.Label($"The README is contained within the file you downloaded from GitHub {Environment.NewLine}and the Wiki is located at the Github. However, you can just click {Environment.NewLine}the Wiki button above to go there.");

        GUILayout.Label($"This is a college project so updates might not happen, {Environment.NewLine}however feel free to send any bugs to my twitter.");
        if (GUILayout.Button(new GUIContent("Twitter", "Opens the developers twitter page, send a message to report bugs."), GUILayout.Width(200f), GUILayout.Height(30f)))
        {
            Application.OpenURL(twitterURL);
        }

        GUILayout.EndArea();
        GUILayout.EndVertical();

        #endregion

        GUILayout.EndHorizontal();

        #region Close Button
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("Close", "Closes this window"), GUILayout.Width(200f), GUILayout.Height(30f)))
        {
            window.Close();
        }

        GUILayout.EndHorizontal();
        #endregion
    }
}
