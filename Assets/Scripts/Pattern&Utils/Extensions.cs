using System.Collections.Generic;
using UnityEngine;

namespace GameDevTVJam2023.TheBedroom.Extension
{
    /// <summary>
    /// Extensions class for usefull methods
    /// </summary>
    internal sealed class Extensions : MonoBehaviour
    {
        /// <summary>
        /// Call this method to destroy all children from a GameObject
        /// Work in editor and play mode
        /// </summary>
        /// <param name="parentTransform">The transform component of the parent GameObject</param>
        public static void DestroyAllChildren(Transform parentTransform)
        {
            List<GameObject> children = new();

            foreach (Transform child in parentTransform)
            {
                children.Add(child.gameObject);
            }

#if UNITY_EDITOR
            children.ForEach(DestroyImmediate);
#else
    children.ForEach(Destroy);
#endif

            children.Clear();
        }
    }
}
