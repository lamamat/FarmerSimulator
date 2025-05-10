using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FindDataItem))]
public class FindDataItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FindDataItem findDataItem = (FindDataItem)target;

        if (GUILayout.Button("Populate Items"))
        {
            findDataItem.items.Clear();

            // Find all BaseItem_Scriptable assets in the project
            string[] guids = AssetDatabase.FindAssets("t:BaseItem_Scriptable");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BaseItem_Scriptable item = AssetDatabase.LoadAssetAtPath<BaseItem_Scriptable>(path);
                if (item != null)
                {
                    // Create a new Data object and populate it
                    Data data = new Data
                    {
                        ID = item.getID(), // Ensure getID() exists in BaseItem_Scriptable
                        item = item
                    };

                    findDataItem.items.Add(data);
                    Debug.Log($"Found BaseItem_Scriptable: {item.name}, ID: {data.ID}");
                }
            }

            // Sort the items by ID (or any other property)
            findDataItem.items.Sort((data1, data2) => string.Compare(data1.ID, data2.ID));

            Debug.Log($"Populated and sorted {findDataItem.items.Count} BaseItem_Scriptable items.");
            EditorUtility.SetDirty(findDataItem); // Mark the object as dirty to save changes
        }
    }
}