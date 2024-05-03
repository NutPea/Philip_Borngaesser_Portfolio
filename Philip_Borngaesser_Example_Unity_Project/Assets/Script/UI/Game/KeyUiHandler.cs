using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyUiHandler : MonoBehaviour
{

    public TextMeshProUGUI keyText;
    private PlayerKeyHandler playerKeyHandler;
    void Start()
    {
        playerKeyHandler = OverSceneReceiver.instance.player.GetComponent<PlayerKeyHandler>();
        playerKeyHandler.OnKeyUpdate.AddListener(UpdateKeyAmountEvent);
        keyText.text = playerKeyHandler.currentKeyAmount.ToString();
    }

    private void UpdateKeyAmountEvent(bool gotAdded)
    {
        keyText.text = playerKeyHandler.currentKeyAmount.ToString();
    }


}
