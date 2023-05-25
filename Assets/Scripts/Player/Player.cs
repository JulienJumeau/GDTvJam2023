using GameDevTVJam2023.TheBedroom.Inputs;
using System.Collections;
using System.Collections.Generic;
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
        private const float _rightAngleThreshold = 45f;
        private const float _upAngleThreshold = 135f;
        [SerializeField] private List<GameObject> _spriteGOList;

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
                _inputManager.OnGroundCliked += InputManager_OnGroundClicked;
            }

            else
            {
                _inputManager.OnGroundCliked -= InputManager_OnGroundClicked;
            }
        }

        private void InputManager_OnGroundClicked(Vector2 clickPosition)
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
            // Calculate the direction to the target position
            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;

            // Calculate the normalized direction and movement angle
            Vector3 normalizedDirection = direction.normalized;
            float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg;

            // Set the appropriate animation based on the angle
            SetMovementAnimation(angle);

            while (distance > 0.01f)
            {
                // Calculate the actual movement distance for this frame
                float movementDistance = Mathf.Min(_moveSpeed * Time.deltaTime, distance);

                // Move the character towards the target position
                transform.position += movementDistance * normalizedDirection;

                // Update the distance to the target position
                distance -= movementDistance;

                // Wait for the next frame
                yield return null;
            }

            // Set the final position to avoid precision errors
            transform.position = targetPosition;
        }

        private void SetMovementAnimation(float angle)
        {
            foreach (GameObject go in _spriteGOList)
            {
                go.SetActive(false);
            }

            // Determine the appropriate animation based on the angle
            if (angle is >= (-_rightAngleThreshold) and < _rightAngleThreshold)
            {
                _spriteGOList[3].SetActive(true);   // Right
            }

            else if (angle is >= _rightAngleThreshold and < _upAngleThreshold)
            {
                _spriteGOList[1].SetActive(true);   // Up
            }

            else if (angle is >= _upAngleThreshold or < (-_upAngleThreshold))
            {
                _spriteGOList[2].SetActive(true);   // Left
            }

            else if (angle is >= (-_upAngleThreshold) and < (-_rightAngleThreshold))
            {
                _spriteGOList[0].SetActive(true);   // Down
            }
        }

        #endregion

        #region Events

        #endregion
    }
}
