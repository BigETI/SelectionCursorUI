using UnityEngine;

/// <summary>
/// Selection cursor UI controllers namespace
/// </summary>
namespace SelectionCursorUI.Controllers
{
    /// <summary>
    /// Selection color class
    /// </summary>
    public class SelectionColor : MonoBehaviour
    {
        /// <summary>
        /// Color
        /// </summary>
        [SerializeField]
        private Color color = Color.white;

        /// <summary>
        /// Color
        /// </summary>
        public Color Color
        {
            get => color;
            set => color = value;
        }
    }
}
