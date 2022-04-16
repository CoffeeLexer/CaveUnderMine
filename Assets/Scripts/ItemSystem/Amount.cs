using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Amount : MonoBehaviour
{
    [SerializeField] [Min(1)] public int number;
}
