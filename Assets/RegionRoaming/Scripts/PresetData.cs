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
        
    }
}
