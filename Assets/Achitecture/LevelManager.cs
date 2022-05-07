using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TowersDraft tower_draft;
    [SerializeField] private GameObject canvas_draft;
    [SerializeField] private GameObject canvas_gameUI;
    [SerializeField] private GameObject tower_builder;

    [Header("Level settings")]
    [SerializeField] private int startFire;
    [SerializeField] private int startWater;
    [SerializeField] private int startEarth;
    [SerializeField] private int startAir;
    [SerializeField] private int startMoney;
    [SerializeField] private int startHP;
    [SerializeField] private int startIncome;
    [SerializeField] private int minMobCount;
    [SerializeField] private int maxMobCount;

    private Bank bank;
    private bool isGame;
    private int incomeInterval;

    public delegate void PlayerHPHandler(object sender, int oldHP, int newHP);
    public event PlayerHPHandler PlayerHPHandlerEvent;

    public int PlayerHp { get; private set; }
    public int IncomeInterval => incomeInterval;

    private void Awake()
    {
        this.bank = GetComponent<Bank>();
        incomeInterval = 15;
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private void StartLevel()
    {
        bank.IncreaseBank(this, startMoney);
        bank.IncreaseIncome(this, startIncome);
        IncreasePlayerHP(this, startHP);
        isGame = true;
        StartCoroutine(UpdateIncome());
        bank.IncreaseElementCount(this, 1, startFire);
        bank.IncreaseElementCount(this, 2, startWater);
        bank.IncreaseElementCount(this, 3, startEarth);
        bank.IncreaseElementCount(this, 4, startAir);
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1.0f);
        StartLevel();
    }

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
        while (isGame)
        {
            yield return new WaitForSeconds(incomeInterval);
            bank.IncreaseBank(this, bank.income);
            Debug.Log("INCOME: " + bank.income);
        }
    }
}
