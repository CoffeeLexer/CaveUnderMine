using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ComponentSet : MonoBehaviour
    {
        [SerializeField] public AnimationController animationController;
        [SerializeField] public CharacterController characterController;
    }
}