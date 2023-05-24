using GameDevTVJam2023.TheBedroom.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameDevTVJam2023.TheBedroom.UI
{
    internal sealed class UIMainMenu : GenericSingleton<UIMainMenu>
    {
        #region Fields & Properties

        [Header("* --- Managers Ref --- *")]
        [Space]
        private UIManager _uiManager;

        [Header("* --- UI MainMenu --- *")]
        [Space]
        private UIDocument _uiDocument;
        private VisualElement _uiRoot;
        private Button _buttonPlay;

        #endregion

        #region Unity Methods + Event Sub

        protected override void Awake()
        {
            base.Awake();
            _uiDocument = GetComponent<UIDocument>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _uiManager = UIManager.Instance;

            if (_uiManager != null && _uiDocument != null)
            {
                _uiManager.CurrentLevelUIDoc = _uiDocument;
            }

            Debug.Log(_uiManager);
        }

        // This will ensure the to subscribe to events. 
        private void OnEnable()
        {
            _uiRoot = _uiDocument.rootVisualElement;
            _buttonPlay = _uiRoot.Q<Button>("ButtonPlay");
            EventSubscription(true);
        }

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
                _buttonPlay.clicked += ButtonPlay_clicked;
            }

            else
            {
                _buttonPlay.clicked -= ButtonPlay_clicked;
            }
        }


        #endregion

        #region Custom Methods

        private void ButtonPlay_clicked()
        {
            Debug.Log("Play");
            GameManager.Instance.SceneToLoad = SceneLoadManager.Instance.BedroomScenesList;
        }

        #endregion

        #region Events

        #endregion
    }
}
