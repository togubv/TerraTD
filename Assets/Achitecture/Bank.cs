using System.Collections;
using UnityEngine;

public class Bank : MonoBehaviour
{
    public int count { get; private set; }
    public int income { get; private set; }
    public int countFire { get; private set; }
    public int countWater { get; private set; }
    public int countEarth { get; private set; }
    public int countAir { get; private set; }

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
        int oldCount = 0;
        switch (elementID)
        {
            case 0:
                oldCount = this.countFire;
                this.countFire += count;
                this.ElementCountUpdateHandlerEvent?.Invoke(sender, elementID, oldCount, this.countFire);
                break;
            case 1:
                oldCount = this.countWater;
                this.countWater += count;
                this.ElementCountUpdateHandlerEvent?.Invoke(sender, elementID, oldCount, this.countWater);
                break;
            case 2:
                oldCount = this.countEarth;
                this.countEarth += count;
                this.ElementCountUpdateHandlerEvent?.Invoke(sender, elementID, oldCount, this.countEarth);
                break;
            case 3:
                oldCount = this.countAir;
                this.countAir += count;
                this.ElementCountUpdateHandlerEvent?.Invoke(sender, elementID, oldCount, this.countAir);
                break;
        }
        
    }

    public void DecreaseElementCount(object sender, int elementID, int count)
    {
        int oldCount = 0;
        int newCount = 0;
        switch (elementID)
        {
            case 0:
                if (this.countFire >= count)
                {
                    oldCount = this.countFire;
                    this.countFire -= count;
                    newCount = this.countFire;
                }
                break;
            case 1:
                if (this.countWater >= count)
                {
                    oldCount = this.countWater;
                    this.countWater -= count;
                    newCount = this.countWater;
                }
                break;
            case 2:
                if (this.countEarth >= count)
                {
                    oldCount = this.countEarth;
                    this.countEarth -= count;
                    newCount = this.countEarth;
                }
                break;
            case 3:
                if (this.countAir >= count)
                {
                    oldCount = this.countAir;
                    this.countAir -= count;
                    newCount = this.countAir;
                }
                break;
        }

        this.ElementCountUpdateHandlerEvent?.Invoke(sender, elementID, oldCount, newCount);
        Debug.Log($"from {sender} take {elementID} count: {count}");

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