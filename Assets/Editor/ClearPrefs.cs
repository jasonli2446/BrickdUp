using UnityEditor;
using UnityEngine;

public static class ClearPrefs
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared via Tools menu");
    }
}
