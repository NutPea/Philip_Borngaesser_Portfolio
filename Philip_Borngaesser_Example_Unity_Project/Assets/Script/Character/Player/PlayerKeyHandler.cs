using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerKeyHandler : MonoBehaviour
{
    public int currentKeyAmount = 0;

    /// <summary>
    /// true means adding money false means remove
    /// </summary>
    [HideInInspector] public UnityEvent<bool> OnKeyUpdate = new UnityEvent<bool>();
    [HideInInspector] public UnityEvent OnNotKeyMoney = new UnityEvent();

    private void Start()
    {
        currentKeyAmount = SaveStateManager.instance.GetSaveKeyAmount();
        OnKeyUpdate.Invoke(true);
    }

    public void AddKey(int amount)
    {
        currentKeyAmount += amount;
        OnKeyUpdate.Invoke(true);
        SaveStateManager.instance.SaveKeyAmount(currentKeyAmount);
    }


    public void RemoveKey(int amount)
    {
        if (CanRemoveMoney(amount))
        {
            OnNotKeyMoney.Invoke();
            return;
        }


        currentKeyAmount -= amount;
        OnKeyUpdate.Invoke(false);
        SaveStateManager.instance.SaveKeyAmount(currentKeyAmount);
    }

    bool CanRemoveMoney(int amount)
    {
        return (currentKeyAmount - amount) < 0;
    }
}
