using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static UIManager UIManagerInstance;

    public delegate void OnLifeChanged(float value, float maxValue);
    public delegate void OnCurrencyChanged(int value);
    public delegate void OnGameFinished(bool win);

    public OnLifeChanged LifeChanged;
    public OnCurrencyChanged CurrencyChanged;
    public OnGameFinished GameFinished;

    [SerializeField] List<TowerTemplate> towersTemplates;

    [Header("Game options")]
    [SerializeField] float maxLife;
    [SerializeField] int startingCurrency;

    float currentLife;
    int currentCurrency;
    bool areWavesFinished = false;

    bool isGamePaused = false;

    public void AllRoundFinished() => areWavesFinished = true;

    public int GetCurrentCurrency() => currentCurrency;

    public List<TowerTemplate> TurretTemplates { get => towersTemplates; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        currentLife = maxLife;
        currentCurrency = startingCurrency;


        LifeChanged?.Invoke(currentLife, maxLife);
        CurrencyChanged?.Invoke(currentCurrency);
    }

    private void Update()
    {
        Checks();
    }

    private void Checks()
    {
        if (currentLife <= 0)
        {
            EndGame(false);
        }


        if (areWavesFinished)
        {
            Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

            if (enemies.Length == 0)
            {
                EndGame(true);
            }
        }
    }

    public GameObject GetCloserPot(Vector3 position)
    {
        GameObject closestPot = null;
        float closestDistance = Mathf.Infinity;
        foreach (var pot in FindObjectsByType<Pot>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(position, pot.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPot = pot.gameObject;
            }
        }
        return closestPot;
    }

    public List<TowerTemplate> GetTemplates()
    {
        return towersTemplates;
    }

    public void DoDamage(int damage)
    {
        currentLife -= damage;

        LifeChanged?.Invoke(currentLife, maxLife);

        if (currentLife <= 0)
        {
            EndGame(true);
        }
    }

    public bool CanPurchase(int cost)
    {
        if (currentCurrency >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PurchaseTurret(TowerTemplate turret) 
    {
        bool purchased = false;

        if (currentCurrency >= turret.cost)
        {
            currentCurrency -= turret.cost;
            CurrencyChanged?.Invoke(currentCurrency);
            purchased = true;
        }

        return purchased;
    }

    public void AddCurrency(int value)
    {
        currentCurrency += value;
        CurrencyChanged?.Invoke(currentCurrency);
    }


    private void EndGame(bool win)
    {
        if (win)
        {
            Debug.Log("You Win!");
        }
        else
        {
            Debug.Log("You lose!");
        }

        GameFinished?.Invoke(win);

    }
}
