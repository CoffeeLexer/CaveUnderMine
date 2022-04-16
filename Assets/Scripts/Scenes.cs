using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public static class Scenes
{
    public static void RestartScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
    }

    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void ExitGame()
    {
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public static void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}