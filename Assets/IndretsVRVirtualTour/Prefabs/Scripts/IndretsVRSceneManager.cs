#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IndretsVRVirtualTour
{
    public class IndretsVRSceneManager : MonoBehaviour
    {
        public string targetScene;
        private Plane _plane = new Plane(new Vector3(0f, -30f), -30);
        private Vector3 _screenPosition;
        private Vector3 _worldPosition;
#if UNITY_EDITOR
        private bool _isDragging;
#endif

        private void Start()
        {
            IndretsVRMainManager.Instance.IsFading = true;
        }

        public void Teleport()
        {
            if (IndretsVRMainManager.Instance != null)
            {
                IndretsVRMainManager.Instance.PreviousScene = SceneManager.GetActiveScene().name;
            }
#if UNITY_EDITOR
            Debug.Log(targetScene);
#endif
            StartCoroutine(LoadScene(targetScene));
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Teleport();
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
            {
                _isDragging = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                _screenPosition = Input.mousePosition;

                Ray ray = Camera.main.ScreenPointToRay(_screenPosition);

                if (_plane.Raycast(ray, out float distance))
                {
                    _worldPosition = ray.GetPoint(distance);
                }

                transform.position = _worldPosition;
            }
#endif
        }

        private IEnumerator LoadScene(string targetScene)
        {
            IndretsVRMainManager.Instance.IsFading = true;
            yield return new WaitUntil(() => IndretsVRMainManager.Instance.IsFading == false);
            SceneManager.LoadScene(targetScene);
        }

        private void OnMouseExit()
        {
#if UNITY_EDITOR
            _isDragging = false;
#endif
        }
    }
}
