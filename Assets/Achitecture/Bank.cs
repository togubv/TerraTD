using System.Collections;
using UnityEngine;

public class Bank : MonoBehaviour
{
    public int count { get; private set; }
    public int income { get; private set; }
    private int[] CountElement = new int[4];
    public int[] countElement => CountElement;

    public delegate void BankUpdateHandler(object sender, int oldCount, int newCount); 
    public event BankUpdateHandler BankUpdateHandlerEvent;

    public delegate void IncomeUpdateHandler(object sender, int oldIncome, int newIncome);
    public event IncomeUpdateHandler IncomeUpdateHandlerEvent;

    public delegate void ElementCountUpdateHandler(object sender, int elementID, int oldCount, int newCount);
    public event ElementCountUpdateHandler ElementCountUpdateHandlerEvent;

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

    // 0 - fire, 1 - water, 2 - earth, 3 - air
    public void IncreaseElementCount(object sender, int elementID, int count)
    {
        int oldCount = this.CountElement[elementID];
        this.CountElement[elementID] += count;
        this.ElementCountUpdateHandlerEvent?.Invoke(sender, elementID, oldCount, this.CountElement[elementID]);
    }

    public void DecreaseElementCount(object sender, int elementID, int count)
    {
        if (this.CountElement[elementID] >= count)
        {
            int oldCount = this.CountElement[elementID];
            this.CountElement[elementID] -= count;
            this.ElementCountUpdateHandlerEvent?.Invoke(sender, elementID, oldCount, this.CountElement[elementID]);
            Debug.Log($"from {sender} take {elementID} count: {count}");
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