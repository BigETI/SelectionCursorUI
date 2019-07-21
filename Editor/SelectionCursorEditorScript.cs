using SelectionCursorUI.Controllers;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Selection cursor UI editor namespace
/// </summary>
namespace SelectionCursorUI.Editor
{
    /// <summary>
    /// Selection cursor editor script class
    /// </summary>
    [CustomEditor(typeof(SelectionCursor))]
    public class SelectionCursorEditorScript : UnityEditor.Editor
    {
        /// <summary>
        /// Create selection cursor
        /// </summary>
        [MenuItem("GameObject/UI/Selection Cursor")]
        public static void CreateSelectionCursor()
        {
            SelectionCursorAssetsObjectScript assets = Resources.Load<SelectionCursorAssetsObjectScript>("SelectionCursorAssets");
            if (assets != null)
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
                    if ((assets.CanvasAsset != null) && (assets.EventSystemAsset != null))
                    {
                        GameObject go = Instantiate(assets.CanvasAsset);
                        if (go != null)
                        {
                            go.name = "Canvas";
                            canvas_rect_transform = go.GetComponent<RectTransform>();
                            if (canvas_rect_transform != null)
                            {
                                go = Instantiate(assets.EventSystemAsset);
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
                    if (assets.SelectionCursorAsset != null)
                    {
                        GameObject selection_cursor_game_object = Instantiate(assets.SelectionCursorAsset);
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
        }
    }
}
