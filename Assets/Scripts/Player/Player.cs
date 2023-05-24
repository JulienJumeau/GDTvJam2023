using GameDevTVJam2023.TheBedroom.Inputs;
using System.Collections;
using UnityEngine;

namespace GameDevTVJam2023.TheBedroom.Player
{
    internal sealed class Player : GenericSingleton<Player>
    {
        #region Fields & Properties

        [Header("* --- Managers Ref --- *")]
        [Space]
        private InputManager _inputManager;

        [Header("* --- Character Moves --- *")]
        [Space]
        [SerializeField] private float _moveSpeed = 0f;
        private Coroutine _moveCoroutine;

        #endregion

        #region Unity Methods + Event Sub

        protected override void Awake()
        {
            base.Awake();
            _inputManager = InputManager.Instance;
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
                _inputManager.OnGroundCliked += _inputManager_OnGroundCliked;
            }

            else
            {
                _inputManager.OnGroundCliked -= _inputManager_OnGroundCliked;
            }
        }

        private void _inputManager_OnGroundCliked(Vector2 clickPosition)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            _moveCoroutine = StartCoroutine(MovePlayerTo(clickPosition));
        }

        #endregion

        #region Custom Methods

        private IEnumerator MovePlayerTo(Vector3 targetPosition)
        {
            // Calculate the distance between the current position and the target position
            float distance = Vector3.Distance(transform.position, targetPosition);

            while (distance > 0.01f)
            {
                // Move the character towards the target position
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);

                // Recalculate the distance between the current position and the target position
                distance = Vector3.Distance(transform.position, targetPosition);

                // Wait for the next frame
                yield return null;
            }
        }

        #endregion

        #region Events

        #endregion
    }
}
