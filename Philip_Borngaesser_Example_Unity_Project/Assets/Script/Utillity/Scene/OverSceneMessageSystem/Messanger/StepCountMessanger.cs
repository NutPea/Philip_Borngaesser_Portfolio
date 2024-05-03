using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCountMessanger : AMessange
{
    public int lastStepAmount = 0;

    public override void Clear(OverSceneMessanger messanger, OverSceneReceiver receiver)
    {
        Destroy(this);
    }

    public override void UseAwake(OverSceneReceiver receiver)
    {
        
    }

    public override void UseLateStart(OverSceneReceiver receiver)
    {

    }
    public override void UseStart(OverSceneReceiver receiver)
    {
        PlayerStepHandler playerStepHandler = receiver.player.GetComponent<PlayerStepHandler>();
        playerStepHandler.currentStepAmount = lastStepAmount;
        playerStepHandler.onStepUpdate.Invoke();
    }
}
