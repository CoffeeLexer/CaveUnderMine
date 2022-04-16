using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoldTracker : MonoBehaviour
{
    [SerializeField] [Min(0)] private int trueAmount;
    [SerializeField] private Text UIText;
    [SerializeField] [Range(0.01f, 1)] private float speed;
    [SerializeField] [Min(0)] private int skippedFrames;
    private int currentSkip = 0;
    private int currentAmount = 0;

    private void FixedUpdate()
    {
        currentSkip++;
        if (currentSkip > skippedFrames)
        {
            currentSkip = 0;
            if (trueAmount != currentAmount)
            {
                float t;
                // Goes up
                if (trueAmount > currentAmount)
                {
                    t = 0.5f + 0.5f * speed;
                }
                // Goes down
                else
                {
                    t = 0.5f - 0.5f * speed;
                }
                currentAmount = (int)math.lerp((float)currentAmount, (float)trueAmount, t);
                UIText.text = currentAmount.ToString();
            }
        }
    }

    public void AddGold(int amount)
    {
        trueAmount += amount;
    }
}
