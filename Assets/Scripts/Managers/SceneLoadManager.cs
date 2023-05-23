using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameDevTVJam2023.TheBedroom.Managers
{
    internal sealed partial class SceneLoadManager : GenericSingleton<SceneLoadManager>
    {
        #region Fields & Properties

        [Header("* --- Scenes List --- *")]
        [Space]
        private readonly List<GameScene> _mainMenuScenesList = new() { GameScene.MainMenu };
        private readonly List<GameScene> _bedroomScenesList = new() { GameScene.BedroomArt };

        internal IReadOnlyList<GameScene> MainMenuScenesList => _mainMenuScenesList.AsReadOnly();
        internal IReadOnlyList<GameScene> BedroomScenesList => _bedroomScenesList.AsReadOnly();
        internal bool IsGameScenesLoaded => SceneManager.sceneCount != 1;

        #endregion

        #region Unity Methods + Event Sub

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        #region Custom Methods

        internal IEnumerator LoadSceneCoroutine(IReadOnlyList<GameScene> scenesToLoad)
        {
            foreach (GameScene scene in scenesToLoad)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
                // wait until the asynchronous scene fully loads
                while (!asyncLoad.isDone)
                    yield return null;
            }
        }

        internal IEnumerator UnloadSceneCoroutine()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (i != 0)
                {
                    AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                    // wait until the asynchronous scene fully unloads
                    while (!asyncUnload.isDone)
                        yield return null;
                }
            }
        }

        #endregion

        #region Events

        #endregion
    }
}
