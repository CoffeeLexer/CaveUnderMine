using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    private SceneFader sceneFader;

    private bool changingScenes = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground") sound.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Contact");
    }
}
