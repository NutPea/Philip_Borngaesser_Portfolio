using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.localEulerAngles;
        rotation.z += rotateSpeed * Time.deltaTime;
        transform.localEulerAngles = rotation;
    }
}
