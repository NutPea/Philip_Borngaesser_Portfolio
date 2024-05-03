using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleHandler : MonoBehaviour
{
    static string myLog = "";
    private string output;
    private string stack;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    private void Start()
    {
        InputManager.instance.inputActions.Keyboard.Console.performed += ctx => ToggleConsole();
    }

    bool toggleConsole;

    void ToggleConsole()
    {
        if (toggleConsole)
        {
            toggleConsole = false;
        }
        else
        {
            toggleConsole = true;
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        myLog = output + "\n" + myLog;
        if (myLog.Length > 5000)
        {
            myLog = myLog.Substring(0, 4000);
        }
    }

    void OnGUI()
    {
        if (toggleConsole) 
        {
            myLog = GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), myLog);
        }
    }

}
