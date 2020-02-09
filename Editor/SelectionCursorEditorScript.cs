using SelectionCursorUI.Controllers;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Selection cursor UI editor namespace
/// </summary>
namespace SelectionCursorUIEditor
{
    /// <summary>
    /// Selection cursor editor script class
    /// </summary>
    [CustomEditor(typeof(SelectionCursor))]
    public class SelectionCursorEditorScript : Editor
    {
        /// <summary>
        /// Create selection cursor
        /// </summary>
        [MenuItem("GameObject/UI/Selection Cursor")]
        public static void CreateSelectionCursor()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(SelectionCursorAssetsObjectScript).Name);
            if (guids != null)
            {
                if (guids.Length > 0)
                {
                    if (guids.Length > 1)
                    {
                        Debug.LogWarning("More than one \"" + typeof(SelectionCursorAssetsObjectScript).Name + "\" has been found.");
                    }
                    string selection_cursor_assets_path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    if (selection_cursor_assets_path != null)
                    {
                        SelectionCursorAssetsObjectScript selection_cursor_assets = AssetDatabase.LoadAssetAtPath<SelectionCursorAssetsObjectScript>(selection_cursor_assets_path);
                        if (selection_cursor_assets != null)
                        {
                            RectTransform canvas_rect_transform = null;
                            GameObject[] game_objects = Selection.gameObjects;
                            if (game_objects != null)
                            {
                                foreach (GameObject game_object in game_objects)
                                {
                                    if (game_object != null)
                                    {
                                        Canvas c = game_object.GetComponent<Canvas>();
                                        if (c != null)
                                        {
                                            canvas_rect_transform = game_object.GetComponent<RectTransform>();
                                            if (canvas_rect_transform != null)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (canvas_rect_transform == null)
                            {
                                if ((selection_cursor_assets.CanvasAsset != null) && (selection_cursor_assets.EventSystemAsset != null))
                                {
                                    GameObject go = Instantiate(selection_cursor_assets.CanvasAsset);
                                    if (go != null)
                                    {
                                        go.name = "Canvas";
                                        canvas_rect_transform = go.GetComponent<RectTransform>();
                                        if (canvas_rect_transform != null)
                                        {
                                            go = Instantiate(selection_cursor_assets.EventSystemAsset);
                                            if (go != null)
                                            {
                                                go.name = "EventSystem";
                                            }
                                            else
                                            {
                                                DestroyImmediate(canvas_rect_transform.gameObject);
                                                canvas_rect_transform = null;
                                            }
                                        }
                                        else
                                        {
                                            DestroyImmediate(go);
                                        }
                                    }
                                    go.AddComponent<Canvas>();
                                }
                            }
                            if (canvas_rect_transform != null)
                            {
                                if (selection_cursor_assets.SelectionCursorAsset != null)
                                {
                                    GameObject selection_cursor_game_object = Instantiate(selection_cursor_assets.SelectionCursorAsset);
                                    if (selection_cursor_game_object != null)
                                    {
                                        selection_cursor_game_object.name = "Selection Cursor";
                                        RectTransform selection_cursor_rect_transform = selection_cursor_game_object.GetComponent<RectTransform>();
                                        if (selection_cursor_rect_transform != null)
                                        {
                                            selection_cursor_rect_transform.SetParent(canvas_rect_transform, true);
                                            selection_cursor_rect_transform.anchoredPosition = Vector2.zero;
                                            selection_cursor_rect_transform.localScale = Vector3.one;
                                        }
                                        else
                                        {
                                            DestroyImmediate(selection_cursor_game_object);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("Selection cursor UI assets is null");
                        }
                    }
                    else
                    {
                        Debug.LogError("Selection cursor UI assets asset path is null");
                    }
                }
                else
                {
                    Debug.LogError("No selection cursor UI assets found");
                }
            }
            else
            {
                Debug.LogError("No selection cursor UI assets found");
            }
        }
    }
}
