using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMoneyHandler : MonoBehaviour
{
    public int currentMoneyAmount = 100;

    /// <summary>
    /// true means adding money false means remove
    /// </summary>
    [HideInInspector]public UnityEvent<bool> OnMoneyUpdate = new UnityEvent<bool>();
    [HideInInspector]public UnityEvent OnNotEnoughMoney = new UnityEvent();

    private void Start()
    {
        currentMoneyAmount = SaveStateManager.instance.GetSaveMoneyAmount();
        OnMoneyUpdate.Invoke(true);
    }

    public void AddMoney(int amount)
    {
        currentMoneyAmount += amount;
        OnMoneyUpdate.Invoke(true);
        SaveStateManager.instance.SaveMoneyAmount(currentMoneyAmount);
    }


    public void RemoveMoney(int amount)
    {
        if (CanRemoveMoney(amount))
        {
            OnNotEnoughMoney.Invoke();
            return;
        }
        currentMoneyAmount -= amount;
        OnMoneyUpdate.Invoke(false);
        SaveStateManager.instance.SaveMoneyAmount(currentMoneyAmount);
    }

    bool CanRemoveMoney(int amount)
    {
        return (currentMoneyAmount - amount) < 0;
    }

}
