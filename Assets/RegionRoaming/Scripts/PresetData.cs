using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PresetData
{
    //Variables
    public List<string> presetName;
    public List<List<float[]>> corners;

    //Struct
    public PresetData(RegionManager regionManager)
    {
        presetName = new List<string>();
        corners = new List<List<float[]>>();

        Dictionary<string, List<Vector3>> presetData = regionManager.presets;
        foreach(string PresetName in presetData.Keys)
        {
            Debug.Log($"{PresetName}");
            presetName.Add(PresetName);

            List<Vector3> corners = presetData[PresetName];
            List<float[]> saveCorners = new List<float[]>();

            foreach (Vector3 corner in corners)
            {
                float[] cornervalue = new float[]
                {
                    corner.x,
                    corner.y,
                    corner.z
                };

                saveCorners.Add(cornervalue);

            }
            this.corners.Add(saveCorners);
        }
    }
}
