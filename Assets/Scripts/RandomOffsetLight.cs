using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class RandomOffsetLight : MonoBehaviour
{
    private void Awake()
    {
        var anim = GetComponent<Animator>();
        if (anim != null) anim.SetFloat("Offset", (float) (new Random().NextDouble() * 30.0));
    }
}
