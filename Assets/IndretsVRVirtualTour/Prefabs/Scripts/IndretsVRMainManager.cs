using UnityEngine;

namespace IndretsVRVirtualTour
{
    public class IndretsVRMainManager : MonoBehaviour
    {
        public static IndretsVRMainManager Instance = null;
        public float mouseSensitivity = 2f;
        private string _previousScene;
        private bool _isFading = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }

        public string PreviousScene
        {
            get { return _previousScene; }
            set { _previousScene = value; }
        }

        public bool IsFading
        {
            get { return _isFading; }
            set { _isFading = value; }
        }
    }
}