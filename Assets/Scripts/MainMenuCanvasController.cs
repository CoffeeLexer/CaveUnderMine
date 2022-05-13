using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuCanvasController : MonoBehaviour
{
    [SerializeField] private RectTransform mainMenu;
    [SerializeField] private RectTransform optionMenu;

    [Header("Scenes")]
    [SerializeField]
    private int menuSceneBuildIndex;
    
    [Header("Menus")]
    [SerializeField] private RectTransform pausedMenu;
    [SerializeField] private RectTransform gameOverMenu;
    [SerializeField] private RectTransform networkMenu;

    [SerializeField] private int networkSceneBuildIndex;
    
    private static void Show(Component component)
    {
        component.gameObject.SetActive(true);
    }

    private static void Hide(Component component)
    {
        component.gameObject.SetActive(false);
    }

    private void Start()
    {
        ShowMainMenu();
    }

    public void StartGame()
    {
        Scenes.LoadNextScene();
    }

    public void ExitGame()
    {
        Scenes.ExitGame();
    }

    public void ShowMainMenu()
    {
        Show(mainMenu);
        Hide(optionMenu);
    }

    public void ShowOptionMenu()
    {
        Hide(mainMenu);
        Show(optionMenu);
    }
    
    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        Scenes.LoadNextScene();
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        Scenes.LoadNextScene();
    }

}
