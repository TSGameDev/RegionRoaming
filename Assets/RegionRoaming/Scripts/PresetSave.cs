using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RegionRoaming.SavePresets
{
    public class PresetSave
    {
        public static void SavePresets(RegionManager regionManager)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/PresetData";
            FileStream stream = new FileStream(path, FileMode.Create);

            PresetData data = new PresetData(regionManager);

            formatter.Serialize(stream, data);

            stream.Close();
        }

        public static PresetData LoadPresets()
        {
            string path = Application.persistentDataPath + "/PresetData";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PresetData data = formatter.Deserialize(stream) as PresetData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError($"Save file not found at {path}");
                return null;
            }
        }
    }
}

