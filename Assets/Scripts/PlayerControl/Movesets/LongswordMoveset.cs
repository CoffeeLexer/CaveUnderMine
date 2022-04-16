using System;
using UnityEngine;

namespace PlayerControl
{
    public class LongswordMoveset : Moveset
    {
        protected override void InitializeMoveset()
        {
            timings = new[] {new Timing(attackSpeed, 0.5f, 0.7f), new Timing(attackSpeed,0.6f, 0.8f )};
            animationSet = Movesets.LongSword;
            animations = 2;
            movementSpeed = 2;
        }

        protected override void ChainSignal()
        {
            Debug.Log("You can chain attacks");
        }

        protected override void OnAttackAnimationEnd()
        {
            Debug.Log("Attack animation ends");
        }
    }
}