using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string SCENES_DIR = "scenes/";

    /// <summary>
    /// Load a scene.
    /// </summary>
    /// <param name="scene">The name of the scene to load.</param>
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(SCENES_DIR + scene);
    }

    /// <summary>
    /// Exit the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
