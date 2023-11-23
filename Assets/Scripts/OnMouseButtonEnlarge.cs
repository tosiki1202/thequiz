using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class OnMouseButtonEnlarge : MonoBehaviour
{
    [SerializeField] float onScale = 1.1f;
    private EventTrigger eventTrigger;
    void Start()
    {
        EventTrigger.Entry entry_Enter = new EventTrigger.Entry();
        eventTrigger = gameObject.AddComponent<EventTrigger>();

        entry_Enter.eventID = EventTriggerType.PointerEnter;
        entry_Enter.callback.AddListener((eventDate) => OnPointEnter());
        eventTrigger.triggers.Add(entry_Enter);

        EventTrigger.Entry entry_Exit = new EventTrigger.Entry();
        entry_Exit.eventID = EventTriggerType.PointerExit;
        entry_Exit.callback.AddListener((eventDate) => OnPointExit());
        eventTrigger.triggers.Add(entry_Exit);

    }
    
    public void OnPointEnter()
    {
        this.transform.localScale = new Vector3(onScale, onScale, onScale);
    }
    public void OnPointExit()
    {
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
