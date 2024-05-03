using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCameraManager : MonoBehaviour
{
    public static MainCameraManager instance;

    public CinemachineVirtualCamera mainCineMachineCam;
    private CinemachineConfiner2D confiner2D;

    private void Awake()
    {
        instance = this;
    }

    public void SetCinemachineConfiner(bool value)
    {
        if (confiner2D == null) confiner2D = mainCineMachineCam.GetComponent<CinemachineConfiner2D>();
        if (confiner2D == null) return;
        //confiner2D.enabled = value;
    }

}
