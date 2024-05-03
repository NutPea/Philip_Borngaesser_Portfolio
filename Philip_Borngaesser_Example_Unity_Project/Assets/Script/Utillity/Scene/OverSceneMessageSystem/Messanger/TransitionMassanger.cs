using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMassanger : AMessange
{
    public override void Clear(OverSceneMessanger messanger, OverSceneReceiver receiver)
    {
        Destroy(this);
    }

    public override void UseAwake(OverSceneReceiver receiver)
    {
    }

    public override void UseStart(OverSceneReceiver receiver)
    {
        receiver.transitionHandler.InTransition();

    }

    public override void UseLateStart(OverSceneReceiver receiver)
    {

    }
}
