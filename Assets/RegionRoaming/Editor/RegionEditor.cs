using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RegionRoaming;

[CustomEditor(typeof(Region))]
public class RegionEditor : Editor
{
    static GameObject regionManager = null;
    Region targetRegion;
    RegionManager RM;
    string presetName = "Enter Preset Name!";

    //chagnes the inspector for the region script
    public override void OnInspectorGUI()
    {
        //stores the selected region into the region variable
        targetRegion = target as Region;
        RM = regionManager.GetComponent<RegionManager>();

        GUILayout.Label("Corners");

        Undo.RecordObject(targetRegion, "Changed Region Settings");

        for (int i = 0; i < targetRegion.Vertices.Count; i++)
        {
            GUILayout.Label($"Element {i}:");

            EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(10f));
            EditorGUI.BeginChangeCheck();

            GUILayout.Label("X: ");
            float x = EditorGUILayout.FloatField(targetRegion.Vertices[i].x);

            GUILayout.Label("Y: ");
            float y = EditorGUILayout.FloatField(targetRegion.Vertices[i].y);

            GUILayout.Label("Z: ");
            float z = EditorGUILayout.FloatField(targetRegion.Vertices[i].z);

            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
            {
                targetRegion.Vertices.RemoveAt(i);
                break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                targetRegion.Vertices[i] = new Vector3(x,y,z);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Corner"))
        {
            targetRegion.Vertices.Add(new Vector3(0, 0, 0));
        }

        if (GUILayout.Button("Add to Presets"))
        {
            RM.presets.Add(presetName, targetRegion.Vertices);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        presetName = EditorGUILayout.TextField(presetName);
        EditorGUILayout.Space(10);
        EditorGUI.EndChangeCheck();
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

    //changes the scene view when a region script object is selected
    private void OnSceneGUI()
    {
        targetRegion = target as Region;

        //makes the lines follow the gameobjects transform
        Transform handleTransform = targetRegion.transform;
        
        List<Vector3> vertTransforms = new List<Vector3>();

        foreach(Vector3 vert in targetRegion.Vertices)
        {
            vertTransforms.Add(handleTransform.TransformPoint(vert));
        }

        //makes the lines white
        Handles.color = Color.white;

        //loops through and draws a line to each point creating the boarder of the region
        for (int i = 0; i < vertTransforms.Count; i++)
        {
            ConnectRegionPoints(vertTransforms, i);
        }

        //adds functionality to handles
        for (int i = 0; i < targetRegion.Vertices.Count; i++)
        {
            HandleFunctionaity(targetRegion, i);
        }
    }

    //function that allows the drawn handles to be moved and bring the vert point with it for better user experience.
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

    //draws a line to all points creating a region
    private void ConnectRegionPoints(List<Vector3> verts, int index)
    {
        if (index == verts.Count - 1)
            Handles.DrawLine(verts[index], verts[0]);
        else
            Handles.DrawLine(verts[index], verts[index + 1]);
    }

    //creates a menu items which gets the prefab from project path and then instantiates it
    [MenuItem("Region Roaming/Create Region", false, 10)]
    private static void CreateRegion()
    {
        EditorGUI.BeginChangeCheck();
        if (regionManager == null)
            regionManager = new GameObject("Region Manager", typeof(RegionManager));
        EditorGUI.EndChangeCheck();

        GameObject newRegion = new GameObject("New Region", typeof(Region));
        newRegion.transform.parent = regionManager.transform;
    }
}
