using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCanvasController : MonoBehaviour
{
    [Header("Menus")] [SerializeField] private RectTransform pausedMenu;
    [SerializeField] private RectTransform gameOverMenu;

    private void Start()
    {
        Hide(pausedMenu);
        Hide(gameOverMenu);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }



    public void ShowGameOverMenu()
    {
        Hide(pausedMenu);
        Show(gameOverMenu);
    }

    public void ResumeGame()
    {
        Hide(pausedMenu);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Scenes.RestartScene();
    }

    public void ExitGame()
    {
        Scenes.LoadPreviousScene();
    }

    public void TogglePauseGame()
    {
        if (IsGameOver())
        {
            return;
        }

        if (IsGamePaused())
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Show(pausedMenu);
        Time.timeScale = 0;
    }

    private bool IsGameOver()
    {
        return gameOverMenu.gameObject.activeInHierarchy;
    }

    private bool IsGamePaused()
    {
        return pausedMenu.gameObject.activeInHierarchy;
    }

    private static void UpdateChildCount(Transform container, GameObject prefab, int newCount)
    {
        var childCount = container.childCount;
        var change = Mathf.Abs(childCount - newCount);
        if (childCount < newCount)
        {
            AddChildren(container, prefab, change);
        }
        else
        {
            RemoveChildren(container, change);
        }
    }

    private static void AddChildren(Transform container, GameObject prefab, int count)
    {
        for (var i = 0; i < count; i++)
        {
            Instantiate(prefab, container);
        }
    }

    private static void RemoveChildren(Transform container, int count)
    {
        for (var i = count - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    private static void Show(Component component)
    {
        component.gameObject.SetActive(true);
    }

    private static void Hide(Component component)
    {
        component.gameObject.SetActive(false);
    }
}
