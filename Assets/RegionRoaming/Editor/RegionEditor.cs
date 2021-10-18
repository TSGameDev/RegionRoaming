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
        SetVariables();

        TitleAndUndo();

        DisplayVerts();

        DisplayPresets();
    }

    private void SetVariables()
    {
        targetRegion = target as Region;
        if (RM == null)
            RM = regionManager.GetComponent<RegionManager>();
    }

    private void TitleAndUndo()
    {
        GUILayout.Label("Corners");
        Undo.RecordObject(targetRegion, "Changed Region Settings");
    }

    private void DisplayVerts()
    {
        for (int i = 0; i < targetRegion.Vertices.Count; i++)
        {
            GUILayout.Label($"Element {i}:");

            EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(10f));
            EditorGUI.BeginChangeCheck();

            Vector3 temp = DisplayVector3(i);

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

    private void DisplayPresets()
    {
        AddCornerPresetButtons();

        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();

        presetName = EditorGUILayout.TextField(presetName);

        EditorGUILayout.Space(10);
        EditorGUI.EndChangeCheck();

        DisplayAvailablePresets();
    }

    private void AddCornerPresetButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Corner"))
            targetRegion.Vertices.Add(new Vector3(0, 0, 0));

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
    
    private void OnSceneGUI()
    {
        targetRegion = target as Region;

        Transform handleTransform = targetRegion.transform;
        
        List<Vector3> vertTransforms = new List<Vector3>();

        foreach(Vector3 vert in targetRegion.Vertices)
            vertTransforms.Add(handleTransform.TransformPoint(vert));

        
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

     private void ConnectRegionPoints(List<Vector3> verts, int index)
    {
        if (index == verts.Count - 1)
            Handles.DrawLine(verts[index], verts[0]);
        else
            Handles.DrawLine(verts[index], verts[index + 1]);
    }

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
