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
    private SceneFader sceneFader;
    private bool changingScenes;
    private bool fadeStage;
    private void Awake()
    {
        sceneFader = GetComponent<SceneFader>();
        fadeStage = false;
    }

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
        Hide(mainMenu);
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
        if (changingScenes)
        {
            return;
        }
        StartCoroutine(StartGameDelayed());
    }
    private IEnumerator StartGameDelayed()
    {
        changingScenes = true;
        yield return sceneFader.FadeOutScene();
        Hide(mainMenu);                                 
        NetworkManager.Singleton.StartHost();     
        StartCoroutine(StartGameDelayed2());
    }
    private IEnumerator StartGameDelayed2()
    {
        yield return sceneFader.FadeInScene();
    }
    public void JoinGame()
    {
        Hide(mainMenu);
        NetworkManager.Singleton.StartClient();
    }

}
