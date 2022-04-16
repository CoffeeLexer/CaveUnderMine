using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NudgeTracker : MonoBehaviour
{
    [SerializeField] private GoldTracker tracker;

    private void OnDestroy()
    {
        tracker.AddGold(GetComponent<Amount>().number);
    }
}
