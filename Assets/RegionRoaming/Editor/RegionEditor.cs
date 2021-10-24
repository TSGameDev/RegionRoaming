using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RegionRoaming;
using UnityEngine.Events;

[CustomEditor(typeof(Region))]
public class RegionEditor : Editor
{
    #region Variables 
    //Required Variables
    Region targetRegion;
    RegionManager RM;
    static GameObject regionManager;

    //Preset Required Variables
    string presetName;
    
    //Testing Variables
    int cubesToSpawn;
    bool raycastRegion = false;
    bool flightRegion = false;
    float minTestFlyingHeight;
    float maxTestFlyingHeight;

    #endregion

    //Update function for custom inspectors
    public override void OnInspectorGUI()
    {
        SetVariables();

        TitleAndUndo();

        DisplayVerts();

        DisplayPresets();

        TestRegion();
    }

    #region ONInspectorGUIFunctions

    //Gets and sets the required vriables for the custom inspector to work
    private void SetVariables()
    {
        targetRegion = target as Region;
        if(RM == null && regionManager == null)
        {
            RM = FindObjectOfType<RegionManager>();
            regionManager = RM.gameObject;
        }
        else if(RM == null && regionManager != null)
        {
            RM = regionManager.GetComponent<RegionManager>();
        }
    }

    //Adds a title to the inspector and adds the functionalit of being able to undo changes to the inspector with Ctr Z.
    private void TitleAndUndo()
    {
        GUILayout.Label("Corners");
        Undo.RecordObject(targetRegion, "Changed Region Settings");
    }

    //calls the function for displaying the float field for each vert alongside creating a removal button for each vert within the list. If there are any changes sets them to the correct vert.
    private void DisplayVerts()
    {
        for (int i = 0; i < targetRegion.Vertices.Count; i++)
        {
            GUILayout.Label($"Element {i}:");

            //makes all drawn UI in horizontal to each other instead of verticel. also check for changes to be assigned back to the vert.
            EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(10f));
            EditorGUI.BeginChangeCheck();

            Vector3 temp = DisplayVector3(i);

            //create a button called X which will remove the vert its next to
            if (GUILayout.Button(new GUIContent("X", "Remove Corner"), GUILayout.Width(20), GUILayout.Height(20)))
            {
                targetRegion.Vertices.RemoveAt(i);
                break;
            }

            if (EditorGUI.EndChangeCheck())
                targetRegion.Vertices[i] = temp;

            EditorGUILayout.EndHorizontal();
        }
    }

    //the function responsible for drawing the vert float feilds called in the DisplayVerts function.
    private Vector3 DisplayVector3(int index)
    {
        //Creates a label for the position name and a field to enter a new float
        GUILayout.Label("X: ");
        float x = EditorGUILayout.FloatField(targetRegion.Vertices[index].x);

        GUILayout.Label("Y: ");
        float y = EditorGUILayout.FloatField(targetRegion.Vertices[index].y);

        GUILayout.Label("Z: ");
        float z = EditorGUILayout.FloatField(targetRegion.Vertices[index].z);

        return new Vector3(x, y, z);
    }

    //Displays all preset related information alongside the button for adding a new vert to the list
    private void DisplayPresets()
    {
        AddCornerPresetButtons();

        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        //creates a text field, used for preset names
        presetName = EditorGUILayout.TextField(new GUIContent("Preset Name", "The name you wish for the preset to be saved as") ,presetName);

        //makes some space between elements in the inspector
        EditorGUILayout.Space(10);
        EditorGUI.EndChangeCheck();

        DisplayAvailablePresets();
    }

    //Adds the two buttons for adding another vert to the list of the region and adding the current vert amount/location to a preset for later use.
    private void AddCornerPresetButtons()
    {
        //adds a button to make a new corner
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Add Corner", "Create an additional corner for the region")))
            targetRegion.Vertices.Add(new Vector3(0, 0, 1));

        //add a button to add the current corner layout to a preset
        if (GUILayout.Button(new GUIContent("Add To Presets", "Adds the current amount and layout of corners to a preset for future loading")))
        {
            List<Vector3> temp = new List<Vector3>();
            for (int i = 0; i < targetRegion.Vertices.Count; i++)
            {
                temp.Add(targetRegion.Vertices[i]);
            }
            RM.presets.Add(presetName, temp);
        }
    }

    //displays the name of all avaliable presets alongside two buttons for loading and removing that preset
    private void DisplayAvailablePresets()
    {
        if (RM.presets != null)
        {
            foreach (string preset in RM.presets.Keys)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(preset);

                if (GUILayout.Button("Load Preset", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    RM.presets.TryGetValue(preset, out targetRegion.Vertices);
                }

                if (GUILayout.Button("Remove Preset", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    RM.presets.Remove(preset);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }

    #endregion

    private void OnSceneGUI()
    {
        targetRegion = target as Region;

        //makes a the handle of the gameobject each to its transform
        Transform handleTransform = targetRegion.transform;
        
        //creates a new list of vecotr3s
        List<Vector3> vertTransforms = new List<Vector3>();

        //foreach element in the region list, adds it to the new list
        foreach(Vector3 vert in targetRegion.Vertices)
            vertTransforms.Add(handleTransform.TransformPoint(vert));

        //changes the line color of any draws to white
        Handles.color = Color.white;

       
        for (int i = 0; i < vertTransforms.Count; i++)
        {
            ConnectRegionPoints(vertTransforms, i);
        }

        
        for (int i = 0; i < targetRegion.Vertices.Count; i++)
        {
            HandleFunctionaity(targetRegion, i);
        }
    }

    #region OnSceneGUIFunctions

    //For each vert in the region, draws a line from itself to the next one in the list. If there isn't one, connects to the beginning of the list to complete the loop
    private void ConnectRegionPoints(List<Vector3> verts, int index)
    {
        if (index == verts.Count - 1)
            Handles.DrawLine(verts[index], verts[0]);
        else
            Handles.DrawLine(verts[index], verts[index + 1]);
    }


    //Makes each vert move responding to the handles functionality and makes the handle move along with the vert as well.
    private Vector3 HandleFunctionaity(Region target, int index)
    {
        Vector3 corner = target.transform.TransformPoint(target.Vertices[index]);
        EditorGUI.BeginChangeCheck();
        corner = Handles.DoPositionHandle(corner, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Corner");
            EditorUtility.SetDirty(target);
            target.Vertices[index] = target.transform.InverseTransformPoint(corner);
        }
        return corner;
    }

    #endregion

    //Creates a menu item that spawns a region under a region manager if there is one. If not makes one. Doesn't assing the Region manager script here due to bugs if that script reference is static
    [MenuItem("Region Roaming/Create Region", false, 10)]
    private static void CreateRegion()
    {
        //finds or creates a new regionmanager and stores it
        EditorGUI.BeginChangeCheck();
        RegionManager temp = GameObject.FindObjectOfType<RegionManager>();

        if(temp != null)
            regionManager = temp.gameObject;
        else
            regionManager = new GameObject("Region Manager", typeof(RegionManager));
        EditorGUI.EndChangeCheck();

        //creates a new region and assigns it to the region manager
        GameObject newRegion = new GameObject("New Region", typeof(Region));
        newRegion.transform.parent = regionManager.transform;

        //makes Unity acknowledge the creation of the region for Undoing its creation. Also makes the selected object the newly created region.
        Undo.RegisterCreatedObjectUndo(newRegion, $"Created {newRegion.name}");
        Selection.activeObject = newRegion;
    }

    #region TestingFunctions

    //A function that creates a method of testing the region without running in playmode.
    private void TestRegion()
    {
        TestingSettings();

        //Adds space in the inspector
        GUILayout.Space(10);

        TestingFunctionality();
    }

    //provides the settings for the region based around the different functions.
    private void TestingSettings()
    {
        EditorGUI.BeginChangeCheck();
        //displays a warning if both toggles are active
        if (raycastRegion && flightRegion)
        {
            EditorGUILayout.HelpBox("Region can not be Raycast and Flight region! Please pick one.", MessageType.Warning);
        }

        EditorGUILayout.BeginHorizontal();
        //displays both bools as a toggle with a name and tooltip
        raycastRegion = EditorGUILayout.Toggle(new GUIContent("Is Raycast Region?", "Will the region use raycasts to return Vector3s relative to terrain height?"), raycastRegion);
        flightRegion = EditorGUILayout.Toggle(new GUIContent("Is Flight Region", "Will the region return Vector3s that are in the air?"), flightRegion);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        //if the flight region toogle is true, displays float fields to enter additional required information for testing
        if (flightRegion)
        {
            minTestFlyingHeight = EditorGUILayout.FloatField(new GUIContent("Min Flying Height", "The minimum distance from the ground for the AI to fly."), minTestFlyingHeight);
            maxTestFlyingHeight = EditorGUILayout.FloatField(new GUIContent("Max Flying Height", "The maximum distance from the ground the AI will fly."), maxTestFlyingHeight);
        }
        EditorGUILayout.EndHorizontal();
    }

    //Displays and calls the testing functionality
    private void TestingFunctionality()
    {
        //displays a warning if the number of cubes to spawn is 75 or more
        if (cubesToSpawn >= 75)
        {
            EditorGUILayout.HelpBox("The test function spawns gameobjects, having a high number could cause crashes or problems.", MessageType.Warning);
        }

        //displays an int field to input the amount of cubes to spawn
        EditorGUILayout.BeginHorizontal();
        cubesToSpawn = EditorGUILayout.IntField(new GUIContent("Number of Test Cubes", "The amount of cubes to spawn during the testing of this region. USE CAREFULLY!"), cubesToSpawn);

        EditorGUI.EndChangeCheck();

        //creates a button for calling the function to spawn the cubes in for testing the region
        if (GUILayout.Button("Test Region"))
        {
            TestCubeSpawning();
        }

        EditorGUILayout.EndHorizontal();
    }

    //spawns the test cubes relative to the type of region the user has selected.
    private void TestCubeSpawning()
    {
        //makes all the region variables inistalisationed for testing and creates a single gameobject to store all created cubes in for ease of deletion
        targetRegion.RegionInistalisation();
        GameObject testCubeManager = new GameObject("Test Cube Manager");

        //creates the amount of cubes entered and assigns their position to the ranomd location defined by the toggled region type. Makes the parent of the spawned cube to cube manager.
        for (int i = 0; i < cubesToSpawn; i++)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 destination;

            if (raycastRegion)
                destination = targetRegion.PickRandomRaycastLocation();
            else if(flightRegion)
                destination = targetRegion.PickRandomFlightLocation(minTestFlyingHeight, maxTestFlyingHeight);
            else
                destination = targetRegion.PickRandomLocation();
            
            temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            temp.transform.position = destination;
            temp.transform.parent = testCubeManager.transform;
        }
    }

    #endregion
}
