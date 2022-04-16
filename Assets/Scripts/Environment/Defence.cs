using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Defence : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    private void OnDestroy()
    {
        Instantiate(particle, transform.position, particle.transform.rotation);
    }
}
