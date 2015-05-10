using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class EditorStarter : Editor
{
    static EditorStarter()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
    }
}
