using UnityEngine;
using UnityEngine.UIElements;

namespace GameDevTVJam2023.TheBedroom.Managers
{
    internal sealed class UIManager : GenericSingleton<UIManager>
    {
        #region Fields & Properties

        [Header("* --- UI System --- *")]
        [Space]
        private UIDocument _currentLevelUIDoc;

        internal UIDocument CurrentLevelUIDoc { get => _currentLevelUIDoc; set => _currentLevelUIDoc = value; }

        #endregion

        #region Unity Methods + Event Sub

        // This will ensure the to subscribe to events. 
        private void OnEnable() => EventSubscription(true);

        // This function is called when the behaviour becomes disabled.
        // This is also called when the object is destroyed and can be used for any cleanup code.
        // This will ensure the to unsubscribe to events properly and avoid memory leak.
        private void OnDisable() => EventSubscription(false);

        /// <summary>
        /// Methods called to (un)subcribe to events
        /// </summary>
        /// <param name="mustSubscribe">True : subscribe, False : unsubcribe</param>
        private void EventSubscription(bool mustSubscribe)
        {
            if (mustSubscribe)
            {
                GameManager.Instance.OnSceneChanged += Instance_OnSceneChanged;
            }

            else
            {
                GameManager.Instance.OnSceneChanged -= Instance_OnSceneChanged;
            }
        }

        #endregion

        #region Custom Methods

        private void Instance_OnSceneChanged(SceneLoadManager.GameScene loadedScene)
        {
            switch (loadedScene)
            {
                case SceneLoadManager.GameScene.Permanent:
                    break;
                case SceneLoadManager.GameScene.MainMenu:
                    break;
                case SceneLoadManager.GameScene.BedroomArt:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Events

        #endregion
    }
}
