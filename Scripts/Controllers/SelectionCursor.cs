using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Selection cursor UI controllers namespace
/// </summary>
namespace SelectionCursorUI.Controllers
{
    /// <summary>
    /// UI selection controller script class
    /// </summary>
    public class SelectionCursor : MonoBehaviour
    {
        /// <summary>
        /// Default color
        /// </summary>
        [SerializeField]
        private Color defaultColor = Color.white;

        /// <summary>
        /// Use unscaled time
        /// </summary>
        [SerializeField]
        private bool useUnscaledTime = true;

        /// <summary>
        /// Border size
        /// </summary>
        [SerializeField]
        private Vector2 borderSize = default;

        /// <summary>
        /// Transition time
        /// </summary>
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float transitionTime = 0.125f;

        /// <summary>
        /// Transition curve
        /// </summary>
        [SerializeField]
        private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

        /// <summary>
        /// Elapsed transition time
        /// </summary>
        private float elapsedTransitionTime;

        /// <summary>
        /// Selected rectangle transform
        /// </summary>
        private RectTransform selectedRectTransform;

        /// <summary>
        /// Selected color
        /// </summary>
        private SelectionColor selectedColor;

        /// <summary>
        /// Old selected rectangle transform
        /// </summary>
        private RectTransform oldSelectedRectTransform;

        /// <summary>
        /// Old selected color
        /// </summary>
        private SelectionColor oldSelectedColor;

        /// <summary>
        /// Rectangle transform
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Image
        /// </summary>
        private Image image;

        /// <summary>
        /// Event system
        /// </summary>
        private EventSystem eventSystem;

        /// <summary>
        /// World corners
        /// </summary>
        private static Vector3[] worldCorners = new Vector3[4];

        /// <summary>
        /// Default color
        /// </summary>
        public Color DefaultColor
        {
            get => defaultColor;
            set => defaultColor = value;
        }

        /// <summary>
        /// Evaluated time
        /// </summary>
        public float EvaluatedTime => ((transitionCurve == null) ? ((transitionTime > float.Epsilon) ? Mathf.Clamp(elapsedTransitionTime / transitionTime, 0.0f, 1.0f) : 1.0f) : transitionCurve.Evaluate((transitionTime > float.Epsilon) ? Mathf.Clamp(elapsedTransitionTime / transitionTime, 0.0f, 1.0f) : 1.0f));

        /// <summary>
        /// Selection color
        /// </summary>
        public Color SelectionColor => ((selectedColor == null) ? defaultColor : selectedColor.Color);

        /// <summary>
        /// Old selection color
        /// </summary>
        public Color OldSelectionColor => ((oldSelectedColor == null) ? defaultColor : oldSelectedColor.Color);

        /// <summary>
        /// Cursor color
        /// </summary>
        public Color CursorColor => ((selectedRectTransform == null) ? Color.clear : ((oldSelectedRectTransform == null) ? SelectionColor : Color.Lerp(OldSelectionColor, SelectionColor, EvaluatedTime)));

        /// <summary>
        /// Get center in world position
        /// </summary>
        /// <param name="rectTransform">Rectangle transform</param>
        /// <returns>Center in world position</returns>
        private static Vector3 GetCenterWorldPosition(RectTransform rectTransform)
        {
            Vector3 ret = Vector3.zero;
            if (rectTransform != null)
            {
                rectTransform.GetWorldCorners(worldCorners);
                foreach (Vector3 world_corner in worldCorners)
                {
                    ret += world_corner;
                }
                ret *= 0.25f;
            }
            return ret;
        }

        /// <summary>
        /// Get size
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <returns>Absolute local scale</returns>
        private Vector2 GetSize(RectTransform transform) => ((transform.rect.size * (new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y)))) + borderSize);

        /// <summary>
        /// Hide
        /// </summary>
        private void Hide()
        {
            selectedRectTransform = null;
            selectedColor = null;
            oldSelectedRectTransform = null;
            oldSelectedColor = null;
            elapsedTransitionTime = 0.0f;
        }

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            eventSystem = FindObjectOfType<EventSystem>();
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if ((eventSystem != null) && (rectTransform != null))
            {
                GameObject selected_game_object = eventSystem.currentSelectedGameObject;
                if (selected_game_object == null)
                {
                    Hide();
                }
                else
                {
                    if (selected_game_object.activeInHierarchy)
                    {
                        RectTransform selected_rect_transform = selected_game_object.GetComponent<RectTransform>();
                        if (selected_rect_transform == null)
                        {
                            Hide();
                        }
                        else
                        {
                            SelectionColor selected_color = selected_rect_transform.GetComponent<SelectionColor>();
                            if (selectedRectTransform == null)
                            {
                                oldSelectedRectTransform = null;
                                oldSelectedColor = null;
                                selectedRectTransform = selected_rect_transform;
                                selectedColor = selected_color;
                            }
                            else if (selectedRectTransform.gameObject.GetInstanceID() != selected_game_object.GetInstanceID())
                            {
                                oldSelectedRectTransform = selectedRectTransform;
                                oldSelectedColor = selectedColor;
                                selectedRectTransform = selected_rect_transform;
                                selectedColor = selected_color;
                                elapsedTransitionTime = 0.0f;
                            }
                        }
                    }
                    else
                    {
                        Hide();
                    }
                }
                if (oldSelectedRectTransform != null)
                {
                    elapsedTransitionTime += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                    if (elapsedTransitionTime >= transitionTime)
                    {
                        oldSelectedRectTransform = null;
                        oldSelectedColor = null;
                        elapsedTransitionTime = 0.0f;
                        rectTransform.sizeDelta = GetSize(selectedRectTransform);
                        rectTransform.position = GetCenterWorldPosition(selectedRectTransform);
                    }
                    else
                    {
                        float time = EvaluatedTime;
                        rectTransform.sizeDelta = Vector2.Lerp(GetSize(oldSelectedRectTransform), GetSize(selectedRectTransform), time);
                        rectTransform.position = Vector3.Lerp(GetCenterWorldPosition(oldSelectedRectTransform), GetCenterWorldPosition(selectedRectTransform), time);
                    }
                }
                else if (selectedRectTransform != null)
                {
                    rectTransform.sizeDelta = GetSize(selectedRectTransform);
                    rectTransform.position = GetCenterWorldPosition(selectedRectTransform);
                }
            }
            if (image != null)
            {
                image.color = CursorColor;
            }
        }
    }
}
