using GameDevTVJam2023.TheBedroom.Managers;
using UnityEngine;

namespace GameDevTVJam2023.TheBedroom.Inputs
{
    internal sealed class InputManager : GenericSingleton<InputManager>
    {
        #region Fields & Properties

        [Header("* --- Managers Ref --- *")]
        [Space]
        private InputActionManager _inputActionManager;

        #endregion

        #region Unity Methods + Event Sub

        // Awake is called when the script instance is being loaded (Overridden from GenericSingleton).
        protected override void Awake()
        {
            base.Awake();
            _inputActionManager = new InputActionManager();
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Start()
        {
            _inputActionManager.Enable();
        }

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
                _inputActionManager.Player.MoveAndSearch.performed += MoveAndSearch_performed;
            }

            else
            {
                GameManager.Instance.OnSceneChanged -= Instance_OnSceneChanged;
                _inputActionManager.Player.MoveAndSearch.performed -= MoveAndSearch_performed;
            }
        }

        #endregion

        #region Custom Methods

        private void MoveAndSearch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            Vector3 mousePosition = _inputActionManager.Player.MousePosition.ReadValue<Vector2>();
            Debug.Log(mousePosition);
        }

        private void Instance_OnSceneChanged(SceneLoadManager.GameScene loadedScene)
        {
            switch (loadedScene)
            {
                case SceneLoadManager.GameScene.Permanent:
                    break;
                case SceneLoadManager.GameScene.MainMenu:
                    SwitchInputActionMap(true);
                    break;
                case SceneLoadManager.GameScene.BedroomArt:
                    SwitchInputActionMap(false);
                    break;
                default:
                    break;
            }
        }

        private void SwitchInputActionMap(bool mustEnableUIActionMapInput)
        {
            if (mustEnableUIActionMapInput)
            {
                _inputActionManager.UI.Enable();
                _inputActionManager.Player.Disable();
            }

            else
            {
                _inputActionManager.UI.Disable();
                _inputActionManager.Player.Enable();
            }
        }

        #endregion

        #region Events

        #endregion
    }
}
