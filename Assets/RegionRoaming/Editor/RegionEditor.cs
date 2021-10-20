using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RegionRoaming;

[CustomEditor(typeof(Region))]
public class RegionEditor : Editor
{
    #region Variables 

    string presetName = "Enter Preset Name!";
    static GameObject regionManager = null;
    RegionManager RM;
    Region targetRegion;

    #endregion

    //Update function for custom inspectors
    public override void OnInspectorGUI()
    {
        SetVariables();

        TitleAndUndo();

        DisplayVerts();

        DisplayPresets();
    }

    #region ONInspectorGUIFunctions

    //Gets and sets the required vriables for the custom inspector to work
    private void SetVariables()
    {
        targetRegion = target as Region;
        if (RM == null && regionManager != null)
            RM = regionManager.GetComponent<RegionManager>();
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
            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
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
        presetName = EditorGUILayout.TextField(presetName);

        //makes some space between elements in the inspector
        EditorGUILayout.Space(10);
        EditorGUI.EndChangeCheck();

        DisplayAvailablePresets();
    }

    //Adds the two buttons for adding another vert to the list of the region and adding the current vert amount/location to a preset for later use.
    private void AddCornerPresetButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Corner"))
            targetRegion.Vertices.Add(new Vector3(0, 0, 1));

        if (GUILayout.Button("Add to Presets"))
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
        EditorGUI.BeginChangeCheck();
        if (regionManager == null)
        {
            regionManager = new GameObject("Region Manager", typeof(RegionManager));
        }
        EditorGUI.EndChangeCheck();

        GameObject newRegion = new GameObject("New Region", typeof(Region));
        newRegion.transform.parent = regionManager.transform;
    }

}
