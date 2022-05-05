using System.Collections;
using UnityEngine;

public class Bank : MonoBehaviour
{
    public int count { get; private set; }
    public int income { get; private set; }

    public delegate void BankUpdateHandler(object sender, int oldCount, int newCount); 
    public event BankUpdateHandler BankUpdateHandlerEvent;

    public delegate void IncomeUpdateHandler(object sender, int oldIncome, int newIncome);
    public event IncomeUpdateHandler IncomeUpdateHandlerEvent;

    public void IncreaseBank(object sender, int count)
    {
        int oldCount = this.count;
        this.count += count;

        this.BankUpdateHandlerEvent?.Invoke(sender, oldCount, this.count);
    }

    public void DecreaseBank(object sender, int count)
    {
        if (CheckBankSize(count))
        {
            int oldCount = this.count;
            this.count -= count;

            this.BankUpdateHandlerEvent?.Invoke(sender, oldCount, this.count);
        }
    }

    public void IncreaseIncome(object sender, int count)
    {
        int oldIncome = this.income;
        this.income += count;

        this.IncomeUpdateHandlerEvent?.Invoke(sender, oldIncome, this.income);
    }

    public void DecreaseIncome(object sender, int count)
    {
        if (CheckIncomeSize(count))
        {
            int oldIncome = this.income;
            this.income -= count;

            this.IncomeUpdateHandlerEvent?.Invoke(sender, oldIncome, this.income);
        }
    }

    public bool CheckBankSize(int decreaseCount)
    {
        return this.count >= decreaseCount;
    }

    public bool CheckIncomeSize(int decreaseCount)
    {
        return this.income >= decreaseCount;
    }
}