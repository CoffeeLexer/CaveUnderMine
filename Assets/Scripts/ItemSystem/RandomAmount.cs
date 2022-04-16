using System.Collections;
using Random = System.Random;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Amount))]
public class RandomAmount : MonoBehaviour
{
    [SerializeField] [Min(0)] private int minInclusive = 0;
    [SerializeField] [Min(0)] private int maxExclusive = 0;
    private static Random rng = new Random();
    void Start()
    {
        GetComponent<Amount>().number = rng.Next(minInclusive, maxExclusive);
    }
}
