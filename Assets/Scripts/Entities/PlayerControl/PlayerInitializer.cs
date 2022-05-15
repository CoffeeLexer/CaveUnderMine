using System.Collections;
using System.Collections.Generic;
using Netcode;
using PlayerControl;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject toInitialize;
    void Start()
    {
        // if(toInitialize.TryGetComponent<PlayerController>(out var controller)) controller.enabled = true;
        // if(toInitialize.TryGetComponent<Moveset>(out var moveset)) moveset.enabled = true;
        // if(toInitialize.TryGetComponent<Dodge>(out var dodge)) dodge.enabled = true;
        
    }
}
