using UnityEngine;

/// <summary>
/// Selection cursor UI editor namespace
/// </summary>
namespace SelectionCursorUI.Editor
{
    /// <summary>
    /// Selection cursor assets object script class
    /// </summary>
    internal class SelectionCursorAssetsObjectScript : ScriptableObject
    {
        /// <summary>
        /// Canvas asset
        /// </summary>
        [SerializeField]
        private GameObject canvasAsset = default;

        /// <summary>
        /// Event system asset
        /// </summary>
        [SerializeField]
        private GameObject eventSystemAsset = default;

        /// <summary>
        /// Selection cursor asset
        /// </summary>
        [SerializeField]
        private GameObject selectionCursorAsset = default;

        /// <summary>
        /// Canvas asset
        /// </summary>
        public GameObject CanvasAsset => canvasAsset;

        /// <summary>
        /// Event system asset
        /// </summary>
        public GameObject EventSystemAsset => eventSystemAsset;

        /// <summary>
        /// Selection cursor asset
        /// </summary>
        public GameObject SelectionCursorAsset => selectionCursorAsset;
    }
}
