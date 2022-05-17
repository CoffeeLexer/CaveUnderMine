using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneNext : MonoBehaviour
{
    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    private SceneFader sceneFader;
    private bool changingScenes = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") EndGame();
    }
    public void EndGame()
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
        Scenes.LoadNextScene();
    }
}
