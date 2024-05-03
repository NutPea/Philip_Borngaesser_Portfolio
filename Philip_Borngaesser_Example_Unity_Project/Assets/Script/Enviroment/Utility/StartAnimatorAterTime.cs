using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimatorAterTime : MonoBehaviour
{
    public float minStartTimer = 0;
    public float maxStartTimer = 1;
    public Animator anim;
         
    // Start is called before the first frame update
    IEnumerator Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.enabled = false;
        float randomeTime = Random.Range(minStartTimer, maxStartTimer);
        yield return new WaitForSeconds(randomeTime);
        anim.enabled = true;
    }


}
