using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class CustomEditorProcedures
{
    private static float step = 0.25f;
    private static float scaleStep = 0.5f;
    private static float leftBoundary = -5;
    private static float rightBoundary = 11f;
    private static float upBoundary = 20f;
    private static float downBoundary = 1f;

    [MenuItem("Custom/Place Blocks in Right Chunk _F1")]
    private static void PlaceBlocksInRightChunk()
    {
        var all = SceneView.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("Block"));
        Debug.Log($"Placing all blocks in their right chunks...");
        foreach (var block in all)
        {
            short pos = (short)Mathf.Ceil(block.transform.position.z / 50);
            GameObject parent = GameObject.Find(pos.ToString());
            Undo.SetTransformParent(block.transform, parent.transform, "Changing parent");
        }
    }

    static string _saveChunksAsLevelPath = "Assets/Resources/Prefabs/Chapters/2/3/";

    [MenuItem("Custom/Save Chunks as Level _F2")]
    private static void SaveChunksAsLevel()
    {
        var chunks = SceneView.FindObjectsOfType<GameObject>().Where(obj => obj.tag == "Chunk");
        foreach (var chunk in chunks)
        {

            string localPath = _saveChunksAsLevelPath + chunk.name + ".prefab";

            PrefabUtility.SaveAsPrefabAsset(chunk, localPath);

        }
        Debug.Log("Saved chunks to " + _saveChunksAsLevelPath);
    }


    [MenuItem("Custom/Block Movement/Left _^LEFT")]
    private static void Left()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if ((selected.position.x - (selected.localScale.x / 2)) - step >= leftBoundary)
            {
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x - step, selected.position.y, selected.position.z);
            }
        }
    }
    [MenuItem("Custom/Block Movement/Right _^RIGHT")]
    private static void Right()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if ((selected.position.x + (selected.localScale.x / 2)) + step <= rightBoundary)
            {
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x + step, selected.position.y, selected.position.z);
            }
        }
    }
    [MenuItem("Custom/Block Movement/Forward _^#UP")]
    private static void Forward()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            Undo.RecordObject(selected, "Movement");
            selected.position = new Vector3(selected.position.x, selected.position.y, selected.position.z + step);
        }
    }
    [MenuItem("Custom/Block Movement/Back _^#DOWN")]
    private static void Back()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            Undo.RecordObject(selected, "Movement");
            selected.position = new Vector3(selected.position.x, selected.position.y, selected.position.z - step);
        }
    }
    [MenuItem("Custom/Block Movement/Up _^UP")]
    private static void Up()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if ((selected.position.y + (selected.localScale.y / 2)) + step <= upBoundary)
            {
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x, selected.position.y + step, selected.position.z);
            }
        }
    }
    [MenuItem("Custom/Block Movement/Down _^DOWN")]
    private static void Down()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if ((selected.position.y - (selected.localScale.y / 2)) - step >= downBoundary)
            {
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x, selected.position.y - step, selected.position.z);
            }
        }
    }

    // ===================================
    // ==========SCALING==================
    // ===================================

    [MenuItem("Custom/Block Scale/Right _^L")]
    private static void ScaleRight()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if (((selected.position.x + (selected.localScale.x / 2)) + scaleStep <= rightBoundary))
            {
                Undo.RecordObject(selected, "Scale");
                selected.localScale = new Vector3(selected.localScale.x + scaleStep, selected.localScale.y, selected.localScale.z);
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x + scaleStep / 2, selected.position.y, selected.position.z);
            }
        }
    }
    [MenuItem("Custom/Block Scale/Left _^J")]
    private static void ScaleLeft()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if (selected.localScale.x > step * 2)
            {
                Undo.RecordObject(selected, "Scale");
                selected.localScale = new Vector3(selected.localScale.x - scaleStep, selected.localScale.y, selected.localScale.z);
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x - scaleStep / 2, selected.position.y, selected.position.z);
            }
        }
    }

    [MenuItem("Custom/Block Scale/Up _^I")]
    private static void ScaleUp()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if (((selected.position.y + (selected.localScale.y / 2)) + scaleStep <= upBoundary))
            {
                Undo.RecordObject(selected, "Scale");
                selected.localScale = new Vector3(selected.localScale.x, selected.localScale.y + scaleStep, selected.localScale.z);
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x, selected.position.y + scaleStep / 2, selected.position.z);
            }
        }
    }
    [MenuItem("Custom/Block Scale/Down _^K")]
    private static void ScaleDown()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if (selected.localScale.y > step * 2)
            {
                Undo.RecordObject(selected, "Scale");
                selected.localScale = new Vector3(selected.localScale.x, selected.localScale.y - scaleStep, selected.localScale.z);
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x, selected.position.y - scaleStep / 2, selected.position.z);
            }
        }
    }
    [MenuItem("Custom/Block Scale/Forward _^#I")]
    private static void ScaleForward()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            Undo.RecordObject(selected, "Scale");
            selected.localScale = new Vector3(selected.localScale.x, selected.localScale.y, selected.localScale.z + scaleStep);
            Undo.RecordObject(selected, "Movement");
            selected.position = new Vector3(selected.position.x, selected.position.y, selected.position.z + scaleStep / 2);
        }
    }
    [MenuItem("Custom/Block Scale/Backward _^#K")]
    private static void ScaleBackward()
    {
        foreach (Transform selected in Selection.gameObjects.Select(x => x.transform))
        {
            RoundTransform(selected);
            if (selected.localScale.z > step * 2)
            {
                Undo.RecordObject(selected, "Scale");
                selected.localScale = new Vector3(selected.localScale.x, selected.localScale.y, selected.localScale.z - scaleStep);
                Undo.RecordObject(selected, "Movement");
                selected.position = new Vector3(selected.position.x, selected.position.y, selected.position.z - scaleStep / 2);
            }
        }
    }

    private static void RoundTransform(Transform t)
    {
        int scaleRounder = Mathf.RoundToInt(1 / scaleStep);
        int moveRounder = Mathf.RoundToInt(1 / step);

        Undo.RecordObject(t, "Rounding1");
        t.localScale *= scaleRounder;
        Undo.RecordObject(t, "Rounding2");
        t.localScale = new Vector3(Mathf.Round(t.localScale.x), Mathf.Round(t.localScale.y), Mathf.Round(t.localScale.z));
        Undo.RecordObject(t, "Rounding3");
        t.localScale /= scaleRounder;

        Undo.RecordObject(t, "Rounding4");
        t.position *= moveRounder;
        Undo.RecordObject(t, "Rounding5");
        t.position = new Vector3(Mathf.Round(t.position.x), Mathf.Round(t.position.y), Mathf.Round(t.position.z));
        Undo.RecordObject(t, "Rounding6");
        t.position /= moveRounder;
    }
}
