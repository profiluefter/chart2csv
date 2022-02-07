using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderButton : MonoBehaviour
{
    public string sceneName;
    
    private void OnMouseUpAsButton()
    {
        SceneManager.LoadScene(sceneName);
    }
}
