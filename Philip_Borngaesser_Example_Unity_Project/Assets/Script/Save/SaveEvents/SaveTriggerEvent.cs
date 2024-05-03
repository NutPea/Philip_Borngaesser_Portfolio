using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SaveTriggerEvent : MonoBehaviour
{
    [HideInInspector] public bool eventWasSaved = false;
    public string GUID;
    public bool GUIDGotSet;
    [HideInInspector] public bool firstTimeInSceneTrigger;

    public UnityEvent onApplySaves = new UnityEvent();
    public UnityEvent onSave = new UnityEvent();
    public bool debug;

    IEnumerator Start()
    {
        if (string.IsNullOrEmpty(GUID))
        {
            string positionString = "||| "+transform.position.x.ToString("F4") + "---" + transform.position.y.ToString("F4") + "---" + transform.position.z.ToString("F4") +" - " + SceneManager.GetActiveScene().buildIndex;
            GUID = gameObject.name + positionString;
        }
        yield return new WaitForEndOfFrame();
        if (SaveStateManager.instance.CheckSaveTriggerEventGUID(this))
        {
            onApplySaves.Invoke();
            if (debug)
            {
                Debug.Log("Saved Event got activated");
            }
        } 
    }

    public void SaveEvent()
    {
        SaveStateManager.instance.SaveSaveTriggerEvent(this);
        onSave.Invoke();
    }

    public void SetGUIDEditor()
    {
        if (Application.isPlaying) return;
        GUID = System.Guid.NewGuid().ToString();
    }
}
