using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RegionRoaming;

[CustomEditor(typeof(Region))]
public class RegionEditor : Editor
{

    //chagnes the inspector for the region script
    public override void OnInspectorGUI()
    {
        Region region = (Region)target;
        if (region == null)
            return;

        GUILayout.Label("Corners");

        Undo.RecordObject(region, "Changed Region Settings");

        for (int i = 0; i < region.Vertices.Count; i++)
        {
            GUILayout.Label($"Element {i}:");

            EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(10f));
            EditorGUI.BeginChangeCheck();

            GUILayout.Label("X: ");
            float x = EditorGUILayout.FloatField(region.Vertices[i].x);

            GUILayout.Label("Y: ");
            float y = EditorGUILayout.FloatField(region.Vertices[i].y);

            GUILayout.Label("Z: ");
            float z = EditorGUILayout.FloatField(region.Vertices[i].z);

            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
            {
                region.Vertices.RemoveAt(i);
                break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                region.Vertices[i] = new Vector3(x,y,z);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Corner"))
        {
            region.Vertices.Add(new Vector3(0, 0, 0));
        }

        if (GUILayout.Button("Set to default"))
        {

        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Set as default"))
        {

        }
    }

    //changes the scene view when a region script object is selected
    private void OnSceneGUI()
    {
        //stores the selected region into the region variable
        Region region = target as Region;
        //doesn't run this editor script unless 3 verts are present
        if (region.Vertices.Count <= 2)
            return;

        //makes the lines follow the gameobjects transform
        Transform handleTransform = region.transform;
        
        List<Vector3> vertTransforms = new List<Vector3>();

        foreach(Vector3 vert in region.Vertices)
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
        for (int i = 0; i < region.Vertices.Count; i++)
        {
            HandleFunctionaity(region, i);
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
    private static void SpawningPrefab()
    {
        GameObject regionPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/RegionRoaming/Prefabs/Region.prefab", typeof(GameObject));
        Instantiate(regionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
