using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace SilverTau.NSR.Samples
{
    [RequireComponent(typeof(Camera))]
    public class CameraControl : MonoBehaviour
    {
        public bool usePointerOverUI = false;
        public bool canRotate;

        [SerializeField, Range(-10.0f, 10.0f)] private float xRotationSpeed = 0.8f;
        [SerializeField, Range(-10.0f, 10.0f)] private float yRotationSpeed = 0.8f;

        private Camera _targetCamera;

        private void Start()
        {
            _targetCamera = GetComponent<Camera>();
            if (_targetCamera == null && Camera.main != null)
                _targetCamera = Camera.main;

            var speedMul = 1.0f;
            
#if UNITY_2019_4_OR_NEWER
#if ENABLE_LEGACY_INPUT_MANAGER
            speedMul = 5.0f;  
#endif
#else
            speedMul = 5.0f;  
#endif
            
#if !PLATFORM_STANDALONE && !UNITY_EDITOR
            speedMul /= 10.0f;
#endif
            
            xRotationSpeed *= speedMul;
            yRotationSpeed *= speedMul;
        }

        private void Update()
        {
            if (canRotate)
                RotateCamera();
        }

        private void RotateCamera()
        {
            if (_targetCamera == null) return;

            if (!TryGetPointerDelta(out var delta)) return;

            if (usePointerOverUI && EventSystem.current.IsPointerOverUI()) { return; }

            float xDeg = -delta.x * xRotationSpeed;
            float yDeg = -delta.y * yRotationSpeed;

            _targetCamera.transform.Rotate(new Vector3(yDeg, -xDeg, 0f), Space.Self);

            var e = _targetCamera.transform.eulerAngles;
            e.z = 0f;
            _targetCamera.transform.rotation = Quaternion.Euler(e);
        }

        private bool TryGetPointerDelta(out Vector2 delta)
        {
            delta = Vector2.zero;

#if UNITY_2019_4_OR_NEWER
                   
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                delta = Mouse.current.delta.ReadValue() * Time.deltaTime * 100f;
                return true;
            }
            
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                delta = Touchscreen.current.primaryTouch.delta.ReadValue() * Time.deltaTime * 100f;
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButton(0))
            {
                delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 100f * Time.deltaTime;
                return true;
            }
#endif

#else
            if (Input.GetMouseButton(0))
            {
                delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 100f * Time.deltaTime;
                return true;
            }
#endif
            return false;
        }
    }
}
