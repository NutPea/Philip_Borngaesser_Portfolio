using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [HideInInspector]public PlayerInput inputActions;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            inputActions = new PlayerInput();
            DontDestroyOnLoad(gameObject);
            transform.parent = null;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
