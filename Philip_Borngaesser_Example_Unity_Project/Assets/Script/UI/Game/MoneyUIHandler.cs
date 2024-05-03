using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUIHandler : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI moneyText;
    PlayerMoneyHandler _playerMoneyHandler;

    private void Awake()
    {
        _playerMoneyHandler = player.GetComponent<PlayerMoneyHandler>();    
        _playerMoneyHandler.OnMoneyUpdate.AddListener(OnMoneyGotUpdated);
    }

    private void OnMoneyGotUpdated(bool gotAdded)
    {
        moneyText.text = _playerMoneyHandler.currentMoneyAmount.ToString();
    }


}
