using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpMessanger : AMessange
{
    NPCPickUpHandler npcPickUpHandler;

    public override void Clear(OverSceneMessanger messanger, OverSceneReceiver receiver)
    {
        Destroy(this);
    }

    public override void UseAwake(OverSceneReceiver receiver)
    {
        
    }


    public override void UseStart(OverSceneReceiver receiver)
    {
       
    }
    public override void UseLateStart(OverSceneReceiver receiver)
    {
        npcPickUpHandler.gameObject.SetActive(true);
        PlayerPickUpHandler playerPickUpHandler = OverSceneReceiver.instance.player.GetComponent<PlayerPickUpHandler>();
        npcPickUpHandler.transform.parent = null;
        playerPickUpHandler.PickUP(npcPickUpHandler, npcPickUpHandler.GetComponent<GridMovementController>());


    }

    public void PickUpPreparation(NPCPickUpHandler npcPickUpHandler)
    {
        this.npcPickUpHandler = npcPickUpHandler;
        npcPickUpHandler.transform.parent = null;
        npcPickUpHandler.gameObject.SetActive(false);
        DontDestroyOnLoad(npcPickUpHandler.gameObject);


    }
}
