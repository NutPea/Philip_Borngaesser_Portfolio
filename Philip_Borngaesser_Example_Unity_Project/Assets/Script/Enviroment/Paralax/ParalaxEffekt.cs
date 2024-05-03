using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffekt : MonoBehaviour
{
    private float startXpos;
    private float startYpos;
    private Transform mainCamTransform;
    public float parallaxEffectValue;
    // Start is called before the first frame update
    void Awake()
    {
        startXpos = transform.localPosition.x;
        startYpos = transform.localPosition.y;
        mainCamTransform = Camera.main.transform;
    }

    private void Update()
    {
        float distX = (mainCamTransform.transform.position.x * parallaxEffectValue);
        float distY = (mainCamTransform.transform.position.y * parallaxEffectValue);
        transform.position = new Vector3(startXpos + distX, startYpos + distY, transform.position.z);
    }

}
