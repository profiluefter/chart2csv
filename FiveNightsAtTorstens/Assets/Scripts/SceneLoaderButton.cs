using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Switches the scene when the collider of the object
 * that this component applies to is clicked.
 */
public class SceneLoaderButton : MonoBehaviour
{
    public string sceneName;
    
    private void OnMouseUpAsButton()
    {
        SceneManager.LoadScene(sceneName);
    }
}
