using GameDevTVJam2023.TheBedroom.Managers;
using UnityEngine;

namespace GameDevTVJam2023.TheBedroom.Inputs
{
    internal sealed class InputManager : GenericSingleton<InputManager>
    {
        private enum Layers
        {
            Ground = 6,
            Furnitures = 7,
        }

        #region Fields & Properties

        [Header("* --- Managers Ref --- *")]
        [Space]
        private InputActionManager _inputActionManager;

        [Header("* --- Click Detection --- *")]
        [Space]
        private Camera _mainCamera;
        private int _raycastLayerMask;

        #endregion

        #region Unity Methods + Event Sub

        // Awake is called when the script instance is being loaded (Overridden from GenericSingleton).
        protected override void Awake()
        {
            base.Awake();
            Cursor.lockState = CursorLockMode.Confined;
            _inputActionManager = new InputActionManager();
            _raycastLayerMask = 1 << (int)Layers.Ground | 1 << (int)Layers.Furnitures;
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

            //To change when build
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(origin: ray.origin, direction: ray.direction, distance: Mathf.Infinity, _raycastLayerMask);

            if (hit.collider != null)
            {
                if (hit.collider.transform.gameObject.layer == (int)Layers.Ground)
                {
                    Debug.Log("Ground Hit");
                    OnGroundCliked(hit.point);
                }

                if (hit.collider.transform.gameObject.layer == (int)Layers.Furnitures)
                {
                    Debug.Log("Furnitures Hit");
                }
            }
        }

        private void Instance_OnSceneChanged(SceneLoadManager.GameScene loadedScene)
        {
            _mainCamera = Camera.main;

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

        /// <summary>
        /// Delegate for subscribers method signature : Return type "void" and the loaded scene as args.
        /// </summary>
        internal delegate void GroundCliked(Vector2 clickPosition);

        // Event to call when the scene must be changed.
        internal event GroundCliked OnGroundCliked;

        #endregion
    }
}
