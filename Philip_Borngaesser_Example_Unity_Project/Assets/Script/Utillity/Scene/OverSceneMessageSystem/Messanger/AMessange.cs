using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMessange : MonoBehaviour
{
    public abstract void Clear(OverSceneMessanger messanger, OverSceneReceiver receiver);
    public abstract void UseStart(OverSceneReceiver receiver);
    public abstract void UseLateStart(OverSceneReceiver receiver);
    public abstract void UseAwake(OverSceneReceiver receiver);
}
