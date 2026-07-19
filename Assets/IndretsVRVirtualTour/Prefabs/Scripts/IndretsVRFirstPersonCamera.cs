using System;
using UnityEngine;

namespace IndretsVRVirtualTour
{
    public class IndretsVRFirstPersonCamera : MonoBehaviour
    {
        public Transform person;
        public float cameraFieldOfView = 60;
        public float minVerticalCameraRotation = -33f;
        public float maxVerticalCameraRotation = 33f;

        [Serializable]
        public struct InitialCameraRotation
        {
            public float horizontalCameraRotation;
            public float verticalCameraRotation;
        }

        public InitialCameraRotation initialCameraRotation;

        [Serializable]
        public struct NavigationCameraRotation
        {
            public string previousSceneName;
            public float horizontalCameraRotation;
            public float verticalCameraRotation;
        }


        public NavigationCameraRotation[] navigationCameraRotation;

        private float _cameraVerticalRotation = 0f;
        private bool _isRotating = false;

        void Start()
        {
            var horizontalCameraRotation = initialCameraRotation.horizontalCameraRotation;
            var verticalCameraRotation = Mathf.Clamp(initialCameraRotation.verticalCameraRotation, minVerticalCameraRotation, maxVerticalCameraRotation);
            if (IndretsVRMainManager.Instance != null)
            {
                var previousScene = IndretsVRMainManager.Instance.PreviousScene;
                horizontalCameraRotation = GetHorizontalCameraRotation(previousScene);
                verticalCameraRotation = GetVerticalCameraRotation(previousScene);

            }

            person.transform.Rotate(0.0f, horizontalCameraRotation, 0.0f, Space.World);
            transform.localEulerAngles = Vector3.right * verticalCameraRotation;
            _cameraVerticalRotation = verticalCameraRotation;

            Transform cameraTransform = person.Find("Main Camera");

            if (cameraTransform != null)
            {
                Camera mainCamera = cameraTransform.GetComponent<Camera>();
                mainCamera.fieldOfView = cameraFieldOfView;
            }
        }

        void Update()
        {
            float inputX = Input.GetAxis("Mouse X") * IndretsVRMainManager.Instance.mouseSensitivity;
            float inputY = Input.GetAxis("Mouse Y") * IndretsVRMainManager.Instance.mouseSensitivity;

            if (Input.GetMouseButtonDown(0))
            {
                _isRotating = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isRotating = false;
            }

            if (_isRotating)
            {
                _cameraVerticalRotation -= inputY;
                _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, minVerticalCameraRotation, maxVerticalCameraRotation);
                transform.localEulerAngles = Vector3.right * _cameraVerticalRotation;
                person.Rotate(Vector3.up * inputX);
            }
        }

        private float GetHorizontalCameraRotation(string previousScene)
        {
            for (int i = 0; i < navigationCameraRotation.Length; i++)
            {
                if (navigationCameraRotation[i].previousSceneName == previousScene)
                {
                    return navigationCameraRotation[i].horizontalCameraRotation;
                }
            }

            return initialCameraRotation.horizontalCameraRotation;
        }

        private float GetVerticalCameraRotation(string previousScene)
        {
            for (int i = 0; i < navigationCameraRotation.Length; i++)
            {
                if (navigationCameraRotation[i].previousSceneName == previousScene)
                {
                    return navigationCameraRotation[i].verticalCameraRotation;
                }
            }

            return Mathf.Clamp(initialCameraRotation.verticalCameraRotation, minVerticalCameraRotation, maxVerticalCameraRotation);
        }
    }
}
