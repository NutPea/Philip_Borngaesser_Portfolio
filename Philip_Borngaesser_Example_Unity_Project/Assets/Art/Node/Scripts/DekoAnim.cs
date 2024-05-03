using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekoAnim : MonoBehaviour
{

    public MovementNode observedNode;
    [HideInInspector]public bool isRight;

    public float xPercentage;
    public float yPercentage;
    public float percentage;
    public virtual void Start()
    {

    }

    public virtual void PlayAnim(bool isPlayer)
    {

    }

}
