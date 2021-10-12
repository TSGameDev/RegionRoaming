using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RegionRoaming;

[CustomEditor(typeof(Region))]
public class RegionEditor : Editor
{

    private void OnSceneGUI()
    {
        //stores the selected region into the region variable
        Region region = target as Region;
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
            if(i == vertTransforms.Count - 1)
                Handles.DrawLine(vertTransforms[i], vertTransforms[0]);
            else
                Handles.DrawLine(vertTransforms[i], vertTransforms[i + 1]);
        }

        //adds functionality to handles
        for (int i = 0; i < region.Vertices.Count; i++)
        {
            ShowCorner(region, i);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private Vector3 ShowCorner(Region target, int index)
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
}
