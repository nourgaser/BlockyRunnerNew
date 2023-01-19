using System.Linq;
using UnityEngine;
using UnityEditor;


public class SliceAndCombine : EditorWindow
{
    [MenuItem("Window/Slice & Combine")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SliceAndCombine window = (SliceAndCombine)EditorWindow.GetWindow(typeof(SliceAndCombine));
        window.titleContent = new GUIContent("Slice & Combine");
        window.Show();
    }

    public enum Axes { x, y, z }
    public static Axes axis = Axes.x;
    public static Vector3 mins = new Vector3(1f, 1f, 1f);

    void OnGUI()
    {
        GUILayout.Label("Selected Axis", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Axis"); axis = (Axes)EditorGUILayout.EnumPopup(axis);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Slice")) Slice();
        if (GUILayout.Button("Combine")) Combine();
    }

    [MenuItem("Custom/Slice _F4")]
    public static void Slice()
    {
        Debug.Log("Slicing...");
        var selected = GetAllSelected();
        if (selected.Length <= 0)
        {
            Debug.Log("Cannot slice. Please select at least 1 object.");
            return;
        }

        foreach (var obj in selected)
        {
            var prefab = Instantiate(obj);
            prefab.transform.position = obj.transform.position;
            GameObject copyA, copyB;
            if (axis == Axes.x)
            {
                prefab.transform.localScale = new Vector3(obj.transform.localScale.x / 2, obj.transform.localScale.y, obj.transform.localScale.z);
                copyA = Instantiate(prefab); copyA.name = obj.name;
                copyB = Instantiate(prefab); copyB.name = obj.name;

                Undo.RegisterCreatedObjectUndo(copyA, "slice copyA");
                Undo.RegisterCreatedObjectUndo(copyB, "slice copyB");
                copyA.transform.position -= new Vector3(obj.transform.localScale.x / 4, 0, 0);
                copyB.transform.position += new Vector3(obj.transform.localScale.x / 4, 0, 0);

            }
            else if (axis == Axes.y)
            {
                prefab.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y / 2, obj.transform.localScale.z);
                copyA = Instantiate(prefab); copyA.name = obj.name;
                copyB = Instantiate(prefab); copyB.name = obj.name;

                Undo.RegisterCreatedObjectUndo(copyA, "slice copyA");
                Undo.RegisterCreatedObjectUndo(copyB, "slice copyB");
                copyA.transform.position -= new Vector3(0, obj.transform.localScale.y / 4, 0);
                copyB.transform.position += new Vector3(0, obj.transform.localScale.y / 4, 0);
            }
            else
            {
                prefab.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z / 2);
                copyA = Instantiate(prefab); copyA.name = obj.name;
                copyB = Instantiate(prefab); copyB.name = obj.name;

                Undo.RegisterCreatedObjectUndo(copyA, "slice copyA");
                Undo.RegisterCreatedObjectUndo(copyB, "slice copyB");
                copyA.transform.position -= new Vector3(0, 0, obj.transform.localScale.z / 4);
                copyB.transform.position += new Vector3(0, 0, obj.transform.localScale.z / 4);
            }
            Undo.DestroyObjectImmediate(obj);
            GameObject.DestroyImmediate(prefab);
        }
    }

    [MenuItem("Custom/Combine _F5")]
    public static void Combine()
    {
        var selected = GetAllSelected();
        if (selected.Length <= 1)
        {
            Debug.Log("Cannot combine. Please select at least 2 objects.");
            return;
        }
        var scale = selected[0].transform.localScale;
        if (!selected.All(o => o.transform.localScale == scale))
        {
            Debug.Log("All selected objects must be of the same scale to combine.");
            return;
        }

        Debug.Log("Combining...");
        if (axis == Axes.x)
        {
            var min = selected.Aggregate((a, b) => a.transform.position.x <= b.transform.position.x ? a : b);
            var combined = Instantiate(min);
            combined.name = min.name;
            Undo.RegisterCreatedObjectUndo(combined, "combined copy created");
            float increase = scale.x * (selected.Length - 1);
            combined.transform.localScale += new Vector3(increase, 0, 0);
            combined.transform.position += new Vector3(increase / 2, 0, 0);
        }
        else if (axis == Axes.y)
        {
            var min = selected.Aggregate((a, b) => a.transform.position.y <= b.transform.position.y ? a : b);
            var combined = Instantiate(min);
            combined.name = min.name;
            Undo.RegisterCreatedObjectUndo(combined, "combined copy created");
            float increase = scale.y * (selected.Length - 1);
            combined.transform.localScale += new Vector3(0, increase, 0);
            combined.transform.position += new Vector3(0, increase / 2, 0);
        }
        else
        {
            var min = selected.Aggregate((a, b) => a.transform.position.z <= b.transform.position.z ? a : b);
            var combined = Instantiate(min);
            combined.name = min.name;
            Undo.RegisterCreatedObjectUndo(combined, "combined copy created");
            float increase = scale.z * (selected.Length - 1);
            combined.transform.localScale += new Vector3(0, 0, increase);
            combined.transform.position += new Vector3(0, 0, increase / 2);
        }

        foreach (var o in selected) Undo.DestroyObjectImmediate(o);
    }

    public static GameObject[] GetAllSelected()
    {
        return Selection.gameObjects;
    }
}
