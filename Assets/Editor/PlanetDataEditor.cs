using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanetData))]
public class PlanetDataEditor : Editor
{
    /*
    public override void OnInspectorGUI()
    {
        PlanetData myTarget = (PlanetData)target;
        EditorGUILayout.LabelField("Planet Properties", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Name", myTarget.astroName);
        EditorGUILayout.LabelField("ID", myTarget.planetID.ToString());
        EditorGUILayout.LabelField("Sign", myTarget.sign.ToString());
        EditorGUILayout.LabelField("Degrees", DegMinSecToString(myTarget.longMinSec));
        EditorGUILayout.LabelField("House", myTarget.house.ToString());

        EditorGUILayout.LabelField("Physical Properties", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Latitude", DegMinSecToString(myTarget.latMinSec));
        EditorGUILayout.LabelField("Longitude speed", DegMinSecToString(myTarget.speedLongMinSec));
        EditorGUILayout.LabelField("Average speed °", DegMinSecToString(myTarget.speedAverageLatMinSec));
        EditorGUILayout.DoubleField("Average speed", myTarget.SpeedAverage);



    }*/

    string DegMinSecToString(int[] val)
    {
        string result = val[0].ToString() + "° " + val[1] + "' " + val[2] + "\"";
        return result;
    }
}
