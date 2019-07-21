﻿using UnityEngine;
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
        /// Use unscaled time
        /// </summary>
        [SerializeField]
        private bool useUnscaledTime = true;

        /// <summary>
        /// Border size
        /// </summary>
        [SerializeField]
        private Vector2 borderSize;

        /// <summary>
        /// Opacity
        /// </summary>
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float opacity = 1.0f;

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
        /// Old selected rectangle transform
        /// </summary>
        private RectTransform oldSelectedRectTransform;

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
        /// Hide
        /// </summary>
        private void Hide()
        {
            selectedRectTransform = null;
            oldSelectedRectTransform = null;
            elapsedTransitionTime = 0.0f;
            if (image != null)
            {
                image.color = new Color(image.color.r, image.color.b, image.color.b, 0.0f);
            }
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
                    RectTransform selected_rect_transform = selected_game_object.GetComponent<RectTransform>();
                    if (selected_rect_transform == null)
                    {
                        Hide();
                    }
                    else
                    {
                        if (selectedRectTransform == null)
                        {
                            oldSelectedRectTransform = null;
                            selectedRectTransform = selected_rect_transform;
                            if (image != null)
                            {
                                image.color = new Color(image.color.r, image.color.b, image.color.b, opacity);
                            }
                        }
                        else if (selectedRectTransform.gameObject.GetInstanceID() != selected_game_object.GetInstanceID())
                        {
                            oldSelectedRectTransform = selectedRectTransform;
                            selectedRectTransform = selected_rect_transform;
                            elapsedTransitionTime = 0.0f;
                            if (image != null)
                            {
                                image.color = new Color(image.color.r, image.color.b, image.color.b, opacity);
                            }
                        }
                    }
                }
                if (oldSelectedRectTransform != null)
                {
                    Vector2 offset = selectedRectTransform.rect.center * selectedRectTransform.localScale * 0.5f;
                    elapsedTransitionTime += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                    if (elapsedTransitionTime >= transitionTime)
                    {
                        oldSelectedRectTransform = null;
                        elapsedTransitionTime = 0.0f;
                        rectTransform.sizeDelta = selectedRectTransform.sizeDelta + borderSize;
                        rectTransform.position = new Vector3(selectedRectTransform.position.x + offset.x, selectedRectTransform.position.y + offset.y, selectedRectTransform.position.z);
                        rectTransform.localScale = selectedRectTransform.localScale;
                    }
                    else
                    {
                        float time = transitionCurve.Evaluate((transitionTime > float.Epsilon) ? Mathf.Clamp(elapsedTransitionTime / transitionTime, 0.0f, 1.0f) : 1.0f);
                        Vector2 old_offset = oldSelectedRectTransform.rect.center * oldSelectedRectTransform.localScale * 0.5f;
                        rectTransform.sizeDelta = Vector2.Lerp(oldSelectedRectTransform.sizeDelta + borderSize, selectedRectTransform.sizeDelta + borderSize, time);
                        rectTransform.position = Vector3.Lerp(new Vector3(oldSelectedRectTransform.position.x + old_offset.x, oldSelectedRectTransform.position.y + old_offset.y, oldSelectedRectTransform.position.z), new Vector3(selectedRectTransform.position.x + offset.x, selectedRectTransform.position.y + offset.y, selectedRectTransform.position.z), time);
                        rectTransform.localScale = Vector3.Lerp(oldSelectedRectTransform.localScale, selectedRectTransform.localScale, time);
                    }
                }
                else if (selectedRectTransform != null)
                {
                    rectTransform.sizeDelta = selectedRectTransform.sizeDelta + borderSize;
                    Vector2 center = selectedRectTransform.rect.center * selectedRectTransform.localScale * 0.5f;
                    rectTransform.position = new Vector3(selectedRectTransform.position.x + center.x, selectedRectTransform.position.y + center.y, selectedRectTransform.position.z);
                    rectTransform.localScale = selectedRectTransform.localScale;
                    if (image != null)
                    {
                        image.color = new Color(image.color.r, image.color.b, image.color.b, opacity);
                    }
                }
            }
        }
    }
}
