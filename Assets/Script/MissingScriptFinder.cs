using UnityEngine;
using UnityEditor;

public class MissingScriptFinder : MonoBehaviour
{
    [MenuItem("Tools/Find Missing Scripts in Scene")]
    public static void FindMissingScripts()
    {
        GameObject[] go = GameObject.FindObjectsOfType<GameObject>();
        int count = 0;
        foreach (GameObject g in go)
        {
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.LogWarning($"Missing script in: {g.name}", g);
                    count++;
                }
            }
        }
        Debug.Log($"Total missing scripts: {count}");
    }
}
