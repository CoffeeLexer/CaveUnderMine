using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class NetworkGameplayCanvasController : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField]
    private int menuSceneBuildIndex;

    [SerializeField] private RectTransform pausedMenu;
    [SerializeField] private RectTransform gameOverMenu;
    [SerializeField] private RectTransform networkMenu;
    [SerializeField] private UnityEvent onShowGameOver;
    [SerializeField] private UnityEvent onResumeGame;
    [SerializeField] private UnityEvent onPauseGame;
    
    
    // [SerializeField] private UnityEvent onRespawn;
    
    private void Start()
    {
        Hide(pausedMenu);
        Hide(gameOverMenu);
        onPauseGame.Invoke();
    }

    // private void Update()
    // {
    //     if()
    // }

    public void ShowGameOverMenu()
    {
        Hide(pausedMenu);
        Show(gameOverMenu);
        onShowGameOver.Invoke();
    }

    public void ResumeGame()
    {
        Hide(pausedMenu);
        onResumeGame.Invoke();
    }

    public void ExitGame() => Scenes.LoadScene(menuSceneBuildIndex);

    public void HostGame()
    {
        Hide(networkMenu);
        onResumeGame.Invoke();
        NetworkManager.Singleton.StartHost();
    }

    public void JoinGame()
    {
        Hide(networkMenu);
        onResumeGame.Invoke();
        NetworkManager.Singleton.StartClient();
    }

    public void AddResumeGameListener(UnityAction action) => onResumeGame.AddListener(action);

    public void AddPauseGameListener(UnityAction action) => onPauseGame.AddListener(action);

    public void ClearRuntimeListeners(UnityAction action)
    {
        onResumeGame.RemoveAllListeners();
        onPauseGame.RemoveAllListeners();
    }

    private void PauseGame()
    {
        Show(pausedMenu);
        onPauseGame.Invoke();
    }

    private bool IsGameOver() => gameOverMenu.gameObject.activeInHierarchy;

    private bool IsGamePaused() => pausedMenu.gameObject.activeInHierarchy;

    private bool IsNetwork() => networkMenu.gameObject.activeInHierarchy;

    private static void Show(Component component) => component.gameObject.SetActive(true);

    private static void Hide(Component component) => component.gameObject.SetActive(false);

    private static void UpdateChildCount(Transform container, GameObject prefab, int newCount)
    {
        var childCount = container.childCount;
        var change = Mathf.Abs(childCount - newCount);
        if(childCount < newCount) AddChildren(container, prefab, change);
        else RemoveChildren(container, change);
    }

    private static void AddChildren(Transform container, GameObject prefab, int count)
    {
        for (var i = count - 1; i >= 0; i--)
        {
            Instantiate(prefab, container);
        }
    }

    private static void RemoveChildren(Transform container, int count)
    {
        for (var i = count - 1; i >= 0 && container.childCount > i; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }
}