using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;
    private bool changingScenes = false;

    private void Start()
    {
        StartCoroutine(StartGameDelayed());
    }

    private IEnumerator StartGameDelayed()
    {
        changingScenes = true;
        yield return sceneFader.FadeInScene();
    }
}
