using System.Linq;
using UnityEngine;
using UnityEditor;

public class Surface : EditorWindow
{

    static Material white;
    static Material red;
    static Material skyblue;
    static Material orange;
    static Material blue;
    static Material green;

    private void OnEnable()
    {
        white = (Material)Resources.Load("Materials/white");
        red = (Material)Resources.Load("Materials/red");
        skyblue = (Material)Resources.Load("Materials/skyblue");
        orange = (Material)Resources.Load("Materials/orange");
        blue = (Material)Resources.Load("Materials/blue");
        green = (Material)Resources.Load("Materials/green");
    }

    [MenuItem("Window/Surface")]
    static void Init()
    {
        Surface window = (Surface)EditorWindow.GetWindow(typeof(Surface));
        window.Show();
    }

    enum SurfaceType
    {
        White,
        Red,
        SkyBlue,
        Orange,
        Blue,
        Green
    }

    static SurfaceType selected = SurfaceType.White;

    private void OnGUI()
    {
        GUILayout.Label("Selected surface type", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Material"); selected = (SurfaceType)EditorGUILayout.EnumPopup(selected);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Apply")) SetSurface();
    }

    [MenuItem("Custom/Set surface _F6")]
    public static void SetSurface()
    {
        var all = Selection.gameObjects;
        foreach (var o in all)
        {
            var mr = o.GetComponent<MeshRenderer>();
            Undo.RecordObject(mr, "set surface material");
            mr.material = GetSelectedMaterial();
            Undo.RecordObject(o, "set surface tag");
            o.tag = GetSelectedTag();

            var rb = o.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Undo.RecordObject(rb, "set surface constraints");
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    private static Material GetSelectedMaterial()
    {
        if (selected == SurfaceType.White) return white;
        if (selected == SurfaceType.Red) return red;
        if (selected == SurfaceType.SkyBlue) return skyblue;
        else if (selected == SurfaceType.Orange) return orange;
        else if (selected == SurfaceType.Blue) return blue;
        else if (selected == SurfaceType.Green) return green;
        else return white;
    }

    private static string GetSelectedTag()
    {
        if (selected == SurfaceType.SkyBlue) return "Untagged";
        if (selected == SurfaceType.White) return "Untagged";
        if (selected == SurfaceType.Red) return "Obstacle";
        else if (selected == SurfaceType.Orange) return "Boost";
        else if (selected == SurfaceType.Blue) return "Slow";
        else if (selected == SurfaceType.Green) return "Locked";
        else return "Untagged";
    }
}
