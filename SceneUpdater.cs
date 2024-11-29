using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUpdater : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadNewScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
