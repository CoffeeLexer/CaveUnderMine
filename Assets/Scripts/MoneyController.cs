using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    public MoneyGenerator generator;
    private void OnDestroy()
    {
        generator.AddMoneyRandom();
        generator.GenCoins();
    }
}
