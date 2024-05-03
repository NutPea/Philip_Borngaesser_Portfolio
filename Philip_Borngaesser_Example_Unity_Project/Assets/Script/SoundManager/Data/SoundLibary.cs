using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibary
{
    public enum Music {
        None ,
        Beginning,
        BanStone_Inside ,
        BanStone_Outside,
        Pesture ,
        HauntedCity,
        BrokenCliffs ,
    };

    public enum Ambient {
        None = -1,
        BanStone_Inside = 0,
        BanStone_Outside = 1,
        Pesture = 2,
        HauntedCity = 3,
        BrokenCliffs = 4,
    };
    public enum SFX {
        None,
        OnPlayer_JumpDefault,
        OnPlayer_LandDefault,
        OnPlayer_CantMove,
        OnPlayerInteract_Without,
        OnPlayerInteract_WithSword,
        OnPlayerInteract_WithGloves,
        OnPlayerDamage ,
        OnPlayerDead,
        OnKeyPickUp,
        OnQuestItemPickUp,
        OnPickUpSword,
        OnPickUpGlove,
        OnDropEquipment,
        OnResetEquipment ,
        OnGateOpen ,
        OnNpcTalk ,
        OnNpcHappy ,
        OnUITextPopUpSound,
        OnSpiderMove,
        OnSpiderInteract ,
        OnSpiderDead,
        OnSpiderDamage,
        OnGrasCut,
        OnBirdFly,
        OnSheepHit,
        OnSheepDead,
        OnStoneMove
    }

    public enum CharacterSFX
    {
        None,
    }



}
