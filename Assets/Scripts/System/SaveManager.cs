using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static string directory = "/Geodata/";
    public static string fileName = "_geo.txt";

    public static void StoreNewData(GeoData gd)
    {
        string dir = Application.persistentDataPath + directory;

        // create directory if it doesn't already exist
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(gd);
        File.WriteAllText(dir + gd.DataName + fileName, json);

    }

    public static List<GeoData> LoadFiles()
    {
        List<GeoData> geolist = new List<GeoData>();
        string path = Application.persistentDataPath + directory;

        // create directory if it doesn't already exist
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.*");

        // read each file
        foreach (FileInfo f in info)
        {
            string json = File.ReadAllText(f.ToString());
            GeoData gd = ScriptableObject.CreateInstance<GeoData>();
            JsonUtility.FromJsonOverwrite(json, gd);
            gd.name = gd.DataName;

            geolist.Add(gd);
        }

        return geolist;
    }
}
