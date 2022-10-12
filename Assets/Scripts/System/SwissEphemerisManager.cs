using SwissEphNet;
using System;
using System.IO;
using UnityEngine;

public class SwissEphemerisManager : MonoBehaviour
{
    static public SwissEph swe;

    static SwissEphemerisManager()
    {
        swe = new SwissEph();

        swe.swe_set_ephe_path(@"C:\ephe");
        swe.OnLoadFile += Swe_OnLoadFile;
    }



    static Stream SearchFile(String fileName)
    {
        fileName = fileName.Trim('/', '\\');
        var folders = new string[] {
            };
        foreach (var folder in folders)
        {
            var f = Path.Combine(folder, fileName);
            if (File.Exists(f))
                return new System.IO.FileStream(f, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        }
        return null;
    }

    static void Swe_OnLoadFile(object sender, LoadFileEventArgs e)
    {
        if (e.FileName.StartsWith("[ephe]"))
        {
            e.File = SearchFile(e.FileName.Replace("[ephe]", string.Empty));
        }
        else
        {
            var f = e.FileName;
            if (System.IO.File.Exists(f))
                e.File = new System.IO.FileStream(f, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        }
    }
}
