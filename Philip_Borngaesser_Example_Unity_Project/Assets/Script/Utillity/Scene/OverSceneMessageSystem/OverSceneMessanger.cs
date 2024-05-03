using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverSceneMessanger : MonoBehaviour
{
    public static OverSceneMessanger instance;
    public List<AMessange> messages;

    private void Awake()
    {
        if(instance == null)
        {
            transform.parent = null;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void UseMessagesAwake(OverSceneReceiver receiver)
    {
        foreach (AMessange message in messages)
        {
            message.UseAwake(receiver);
        }
    }


    public void UseMessagesStart(OverSceneReceiver receiver)
    {
        foreach(AMessange message in messages)
        {
            message.UseStart(receiver);
        }
    }

    public void UseMessagesLateStart(OverSceneReceiver receiver)
    {
        foreach (AMessange message in messages)
        {
            message.UseLateStart(receiver);
        }

        foreach (AMessange message in messages)
        {
            message.Clear(this, receiver);
        }
        messages.Clear();
    }

    public void AddMessage(AMessange targetMessage)
    {
        messages.Add(targetMessage);
    }


}
