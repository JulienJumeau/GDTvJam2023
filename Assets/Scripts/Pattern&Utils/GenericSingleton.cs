using UnityEngine;

namespace GameDevTVJam2023.TheBedroom
{
    /// <summary>
    /// Inheritance from Singleton ensures that only once instance
    /// of the GameObject will be present in the scene.
    /// If there are more, they will be destroyed.
    /// </summary>
    /// <typeparam name="T">Component type.</typeparam>
    [DisallowMultipleComponent]
    internal abstract class GenericSingleton<T> : MonoBehaviour where T : Component
    {
        #region Fields & Properties

        // Allow easy access to the single instance of the object who inherit from this script
        public static T Instance { get; private set; } = null;

        #endregion

        #region Unity Methods + Event Sub

        protected virtual void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
        }

        #endregion
    }
}
