using System;
using UnityEngine;

namespace PlayerControl
{
    public class LongswordMoveset : Moveset
    {
        protected override void InitializeMoveset()
        {
            timings = new[]  { 
                new Timing(attackSpeed, 0.6f, 0.4f, 1.2f, 1.3f), 
                new Timing(attackSpeed,0.6f, 0.4f, 1.3f, 1.4f ) };
            firstSwingDelay = 0.2f;
            animationSet = Movesets.LongSword;
            animations = 2;
            movementSpeed = 2;
            newAttackChainDelay = 0.2f;
        }

        protected override void ChainSignal() 
        {
        }

        protected override void OnAttackAnimationEnd()
        {
            
        }
    }
}