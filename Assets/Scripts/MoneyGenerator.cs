using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

public class MoneyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject money;
    [SerializeField] private int moneyAmount;
    [SerializeField] private int minMoney;
    [SerializeField] private int maxMoney;
    [SerializeField] private Transform tiles;

    [SerializeField] private Text text;
    private Random rng;
    private int MoneyHas = 0;
    private void Awake()
    {
        rng = new Random();
        money = GameObject.Instantiate(money);
        money.AddComponent<MoneyController>().generator = this;
        for(int i = 0; i < moneyAmount; i++)
            GenCoins();
    }

    public void AddMoneyRandom()
    {
        Debug.Log("Adding Moneyz");
        MoneyHas += rng.Next(minMoney, maxMoney);
        text.text = $"Gold: {MoneyHas}";
    }
    public void GenCoins()
    {
        Debug.Log("Generating Money");
        int tileNumber = rng.Next(0, tiles.childCount);
        Instantiate(money, tiles.GetChild(tileNumber).position, money.transform.rotation);
    }
}
