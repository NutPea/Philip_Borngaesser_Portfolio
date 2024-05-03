using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePingPongButton : MonoBehaviour
{
    public float scaleTime = 0.75f;
    public float scaleAmount = 1.2f;
    public bool scaleOnEnable;

    private void OnEnable()
    {
        if (!scaleOnEnable) return;
        StartScaling();
    }

    private void OnDisable()
    {
        //LeanTween.cancel(gameObject);
    }
    void Start()
    {
        if (scaleOnEnable)
        {
            StartScaling();
        }
    }

    public void StartScaling()
    {
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount, 1), scaleTime).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
