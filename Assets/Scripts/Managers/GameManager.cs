using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTVJam2023.TheBedroom.Managers
{
    // Make sure that manager scripts run before others to avoid errors (event subecription for exemple)
    [DefaultExecutionOrder(-1)]
    internal sealed class GameManager : GenericSingleton<GameManager>
    {
        #region Fields & Properties

        [Header("* --- Managers Ref --- *")]
        [Space]
        private SceneLoadManager _sceneLoadManager;

        [Header("* --- Scene Change System --- *")]
        [Space]
        private IReadOnlyList<SceneLoadManager.GameScene> _sceneToLoad;

        internal IReadOnlyList<SceneLoadManager.GameScene> SceneToLoad
        {
            get => _sceneToLoad;
            set
            {
                _sceneToLoad = value;
                _ = StartCoroutine(SceneChangeCoroutine(_sceneToLoad));
            }
        }

        #endregion

        #region Unity Methods + Event Sub

        // Awake is called when the script instance is being loaded (Overridden from GenericSingleton).
        protected override void Awake()
        {
            base.Awake();
            // Capped the framerate to 60 for performance purpose.
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            _sceneLoadManager = SceneLoadManager.Instance;

            if (_sceneLoadManager == null) { return; }

            if (!_sceneLoadManager.IsGameScenesLoaded)
            {
                SceneToLoad = _sceneLoadManager.MainMenuScenesList;
            }
        }

        #endregion

        #region Custom Methods

        private IEnumerator SceneChangeCoroutine(IReadOnlyList<SceneLoadManager.GameScene> scenesToLoad)
        {
            yield return null;
            yield return _sceneLoadManager.StartCoroutine(_sceneLoadManager.UnloadSceneCoroutine());
            yield return _sceneLoadManager.StartCoroutine(_sceneLoadManager.LoadSceneCoroutine(scenesToLoad));
            OnSceneChanged?.Invoke(scenesToLoad[0]);
        }

        #endregion

        #region Events

        /// <summary>
        /// Delegate for subscribers method signature : Return type "void" and the loaded scene as args.
        /// </summary>
        /// <param name="loadedScene">Current Loaded Scene</param>
        internal delegate void SceneChanged(SceneLoadManager.GameScene loadedScene);

        // Event to call when the scene must be changed.
        internal event SceneChanged OnSceneChanged;

        #endregion
    }
}

/*
        #region Fields & Properties

        #endregion

        #region Unity Methods + Event Sub

        #endregion

        #region Custom Methods

        #endregion

        #region Events

        #endregion
*/