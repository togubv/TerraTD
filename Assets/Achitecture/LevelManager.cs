using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas_gameUI;
    [SerializeField] private GameObject tower_builder;

    [Header("Level settings")]
    [SerializeField] private int incomeInterval;
    [Tooltip("Fire/Water/Earth/Air")]
    [SerializeField] private int[] startElement;
    [SerializeField] private int startMoney;
    [SerializeField] private int startHP;
    [SerializeField] private int startIncome;
    [SerializeField] private int minMobCount;
    [SerializeField] private int maxMobCount;

    private Bank bank;
    private bool isGame;
    private int incomeTimer;

    public delegate void PlayerHPHandler(object sender, int oldHP, int newHP);
    public event PlayerHPHandler PlayerHPHandlerEvent;

    public delegate void SliderIncomeIndicator(int newValue, int maxValue);
    public event SliderIncomeIndicator SliderIncomeIndicatorEvent;

    public int PlayerHp { get; private set; }
    public int IncomeInterval => incomeInterval;
    public int IncomeTimer => incomeTimer;

    private void Awake()
    {
        this.bank = GetComponent<Bank>();
    }

    private void Start()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        bank.IncreaseBank(this, startMoney);
        bank.IncreaseIncome(this, startIncome);
        IncreasePlayerHP(this, startHP);
        isGame = true;
        StartCoroutine(UpdateIncome());

        for (int i = 0; i < startElement.Length; i++)
        {
            if (i < startElement.Length) 
                bank.IncreaseElementCount(this, i, startElement[i]);
        }
    }

    //private IEnumerator DelayedStart()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    StartLevel();
    //}

    public void IncreasePlayerHP(object sender, int count)
    {
        int oldHP = this.PlayerHp;
        this.PlayerHp += count;

        PlayerHPHandlerEvent?.Invoke(sender, oldHP, this.PlayerHp);
    }

    public void DecreasePlayerHP(object sender, int count)
    {
        if (CheckPlayerHP(count)) 
        {
            int oldHP = this.PlayerHp;
            this.PlayerHp -= count;
            PlayerHPHandlerEvent?.Invoke(sender, oldHP, this.PlayerHp);
        }
    }

    public bool CheckPlayerHP(int count)
    {
        return this.PlayerHp >= count;
    }

    private IEnumerator UpdateIncome()
    {
        incomeTimer = 1;
        while (isGame)
        {
            yield return new WaitForSeconds(1);
            incomeTimer += 1;
            if (incomeTimer > incomeInterval)
            {
                bank.IncreaseBank(this, bank.income);
                incomeTimer = 1;
                Debug.Log("INCOME: " + bank.income);
            }
            SliderIncomeIndicatorEvent?.Invoke(incomeTimer, incomeInterval);
        }
    }
}
