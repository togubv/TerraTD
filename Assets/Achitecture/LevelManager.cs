using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TowersDraft tower_draft;
    [SerializeField] private GameObject canvas_draft;
    [SerializeField] private GameObject canvas_gameUI;
    [SerializeField] private GameObject tower_builder;

    public Event update_grid;
    public int startMoney, startHP, startIncome;

    private Bank bank;
    //private int[] pool;
    private bool isGame;
    private int incomeInterval;
    private GameObject[] tower_prefab_list;

    public GameObject[] Tower_prefab_list => tower_prefab_list;
    //public int[] Pool => pool;

    public delegate void PlayerHPHandler(object sender, int oldHP, int newHP);
    public event PlayerHPHandler PlayerHPHandlerEvent;

    public int PlayerHp { get; private set; }

    private void Awake()
    {
        this.bank = GetComponent<Bank>();
        incomeInterval = 15;
    }

    private void Start()
    {
        bank.IncreaseBank(this, startMoney);
        bank.IncreaseIncome(this, startIncome);
        IncreasePlayerHP(this, startHP);
        isGame = true;
        StartCoroutine(UpdateIncome());
    }

    public void EndDraft()
    {
        //tower_prefab_list = tower_draft.Towers_list;
        //pool = tower_draft.Pool;

        //tower_builder.SetActive(true);
        //canvas_draft.SetActive(false);
        //canvas_gameUI.SetActive(true);
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
