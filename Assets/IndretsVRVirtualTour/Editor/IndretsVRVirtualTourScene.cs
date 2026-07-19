using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

namespace IndretsVRVirtualTour
{
    public class IndretsVRVirtualTourScene : MonoBehaviour
    {
        [MenuItem("Assets/Create/IndretsVRVirtualTour Scene")]
        private static void CreateCustomOption()
        {
            Object selectedObject = Selection.activeObject;
            Debug.Log(selectedObject);

            if (selectedObject != null && (selectedObject is Texture2D || selectedObject is UnityEngine.Video.VideoClip))
            {
                Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

                GameObject[] rootObjects = newScene.GetRootGameObjects();

                foreach (var obj in rootObjects)
                {
                    if (obj.name == "Main Camera" || obj.name == "Directional Light")
                    {
                        Object.DestroyImmediate(obj, true);
                    }
                }

                GameObject person = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/IndretsVRVirtualTour/Prefabs/Person.prefab");
                GameObject instantiatedPerson = PrefabUtility.InstantiatePrefab(person, newScene) as GameObject;

                GameObject hotspot = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/IndretsVRVirtualTour/Prefabs/Hotspot.prefab");
                GameObject instantiatedHotspot = PrefabUtility.InstantiatePrefab(hotspot, newScene) as GameObject;
                instantiatedHotspot.transform.position = new Vector3(0f, -30f, 60f);
                instantiatedHotspot.transform.localScale = new Vector3(5f, 0.01f, 5f);

                GameObject mainManager = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/IndretsVRVirtualTour/Prefabs/MainManager.prefab");
                GameObject instantiatedMainManager = PrefabUtility.InstantiatePrefab(mainManager, newScene) as GameObject;

                if (selectedObject is Texture2D)
                {
                    Texture2D selectedTexture = (Texture2D)selectedObject;
                    string path = AssetDatabase.GetAssetPath(selectedTexture);
                    TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                    textureImporter.mipmapEnabled = false;
                    textureImporter.maxTextureSize = 8192;

                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    Material material = new Material(Shader.Find("Skybox/Panoramic"));
                    material.SetTexture("_MainTex", selectedTexture);
                    RenderSettings.skybox = material;
                }
                else if (selectedObject is UnityEngine.Video.VideoClip)
                {
                    UnityEngine.Video.VideoClip selectedVideo = (UnityEngine.Video.VideoClip)selectedObject;
                    GameObject videoGameObject = new GameObject("VideoClip");
                    SceneManager.MoveGameObjectToScene(videoGameObject, newScene);
                    VideoPlayer videoPlayer = videoGameObject.AddComponent<VideoPlayer>();
                    RenderTexture renderTexture = new RenderTexture((int)selectedVideo.width, (int)selectedVideo.height, 24);
                    renderTexture.name = "VideoClip";
                    videoPlayer.clip = selectedVideo;
                    videoPlayer.targetTexture = renderTexture;

                    Material material = new Material(Shader.Find("Skybox/Panoramic"));
                    material.SetTexture("_MainTex", renderTexture);
                    RenderSettings.skybox = material;
                }


                EditorSceneManager.MarkSceneDirty(newScene);

                Debug.Log("New IndretsVRVirtualTourScene created.");
            }
            else
            {
                Debug.LogWarning("Please, select a valid image.");
            }
        }

        [MenuItem("Assets/Create/IndretsVRVirtualTour Scene", true)]
        private static bool ValidateCreateCustomOption()
        {
            return Selection.activeObject != null;
        }
    }
}