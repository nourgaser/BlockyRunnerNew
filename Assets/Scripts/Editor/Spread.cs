using System.Linq;
using UnityEngine;
using UnityEditor;

public class Spread : EditorWindow
{
    [MenuItem("Window/Spread")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        Spread window = (Spread)EditorWindow.GetWindow(typeof(Spread));
        window.Show();
    }

    public enum SpreadMode
    {
        Linear,
        Alternate,
        AlternateNegate,
        Random
    }
    public static SpreadMode spreadModeX = SpreadMode.Linear;
    public static SpreadMode spreadModeY = SpreadMode.Linear;
    public static SpreadMode spreadModeZ = SpreadMode.Linear;
    public static Vector3 spreadOffset = Vector3.zero;
    public static bool[] spreadInclude = { false, false, true };
    public static bool[] spreadNegative = { false, false, false };
    public static int numberOfSpreads = 1;

    public static bool spreading = false;
    void OnGUI()
    {
        GUILayout.Label("Spread", EditorStyles.boldLabel);

        GUILayout.Label("Spread type");
        GUILayout.BeginHorizontal();
        GUILayout.Label("x"); spreadModeX = (SpreadMode)EditorGUILayout.EnumPopup(spreadModeX);
        GUILayout.Label("y"); spreadModeY = (SpreadMode)EditorGUILayout.EnumPopup(spreadModeY);
        GUILayout.Label("z"); spreadModeZ = (SpreadMode)EditorGUILayout.EnumPopup(spreadModeZ);
        GUILayout.EndHorizontal();

        GUILayout.Label("Spread direction");
        GUILayout.BeginHorizontal();
        GUILayout.Label("x"); spreadInclude[0] = EditorGUILayout.Toggle(spreadInclude[0]);
        GUILayout.Label("y"); spreadInclude[1] = EditorGUILayout.Toggle(spreadInclude[1]);
        GUILayout.Label("z"); spreadInclude[2] = EditorGUILayout.Toggle(spreadInclude[2]);
        GUILayout.EndHorizontal();

        GUILayout.Label("Spread negative");
        GUILayout.BeginHorizontal();
        GUILayout.Label("x"); spreadNegative[0] = EditorGUILayout.Toggle(spreadNegative[0]);
        GUILayout.Label("y"); spreadNegative[1] = EditorGUILayout.Toggle(spreadNegative[1]);
        GUILayout.Label("z"); spreadNegative[2] = EditorGUILayout.Toggle(spreadNegative[2]);
        GUILayout.EndHorizontal();

        spreadOffset = EditorGUILayout.Vector3Field("Spread offset", spreadOffset);

        numberOfSpreads = EditorGUILayout.IntField("Number of spreads", numberOfSpreads);

        if (GUILayout.Button("Spread"))
        {
            spreading = true;
            _Spread();
        }
    }

    [MenuItem("Custom/Spread _F3")]
    public static void _Spread()
    {
        if (Selection.gameObjects.Length == 0) { Debug.Log("Nothing selected. Cannot spread."); return; }
        if (!((spreadInclude[0]) || (spreadInclude[1]) || (spreadInclude[2]))) { Debug.Log("At least one direction should be included. Cannot spread."); return; }


        foreach (var selected in Selection.gameObjects)
        {
            var currSelected = selected;
            for (int i = 0; i < numberOfSpreads; i++)
            {
                var copy = Instantiate(currSelected);
                copy.name = $"{selected.name}";
                Undo.RegisterCreatedObjectUndo(copy, "creating object for spreading");
                Undo.RecordObject(copy.transform, "spreading");

                Vector3 newPos = currSelected.transform.position;
                if (spreadInclude[0])
                    switch (spreadModeX)
                    {
                        case SpreadMode.Linear:
                            newPos.x += spreadOffset.x + ((spreadNegative[0] ? -1 : 1) * currSelected.transform.localScale.x);
                            break;
                        case SpreadMode.Alternate:
                            newPos.x += i % 2 == 0 ? spreadOffset.x + ((spreadNegative[0] ? -1 : 1) * currSelected.transform.localScale.x) : 0;
                            break;
                        case SpreadMode.AlternateNegate:
                            newPos.x += (i % 2 == 0 ? 1 : -1) * (spreadOffset.x + ((spreadNegative[0] ? -1 : 1) * currSelected.transform.localScale.x));
                            break;
                        case SpreadMode.Random:
                            float width = copy.transform.localScale.x / 2;
                            newPos.x = Mathf.Round(Random.Range(-5f + width, 11f - width) * 4) / 4;
                            break;
                    }

                if (spreadInclude[1])
                    switch (spreadModeY)
                    {
                        case SpreadMode.Linear:
                            newPos.y += spreadOffset.y + ((spreadNegative[1] ? -1 : 1) * currSelected.transform.localScale.y);
                            break;
                        case SpreadMode.Alternate:
                            newPos.y += i % 2 == 0 ? spreadOffset.y + ((spreadNegative[1] ? -1 : 1) * currSelected.transform.localScale.y) : 0;
                            break;
                        case SpreadMode.AlternateNegate:
                            newPos.y += (i % 2 == 0 ? 1 : -1) * (spreadOffset.y + ((spreadNegative[1] ? -1 : 1) * currSelected.transform.localScale.y));
                            break;
                        case SpreadMode.Random:
                            float height = copy.transform.localScale.y / 2;
                            newPos.y = Mathf.Round(Random.Range(1 + height, 20f - height) * 4) / 4;
                            break;
                    }

                if (spreadInclude[2])
                    switch (spreadModeZ)
                    {
                        case SpreadMode.Linear:
                            newPos.z += spreadOffset.z + ((spreadNegative[2] ? -1 : 1) * currSelected.transform.localScale.z);
                            break;
                        case SpreadMode.Alternate:
                            newPos.z += i % 2 == 0 ? spreadOffset.z + ((spreadNegative[2] ? -1 : 1) * currSelected.transform.localScale.z) : 0;
                            break;
                        case SpreadMode.AlternateNegate:
                            newPos.z += (i % 2 == 0 ? 1 : -1) * (spreadOffset.z + ((spreadNegative[2] ? -1 : 1) * currSelected.transform.localScale.z));
                            break;
                        case SpreadMode.Random:
                            newPos.z += (spreadNegative[2] ? -1 : 1) * Mathf.Round(Random.Range(0.25f, Mathf.Max(Mathf.Abs(spreadOffset.z), 0.5f)) * 4) / 4 + ((spreadNegative[2] ? -1 : 1) * currSelected.transform.localScale.z);
                            break;
                    }

                copy.transform.position = newPos;
                currSelected = copy;
            }
        }

        spreading = false;
    }
}
