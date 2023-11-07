using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnMouseColorChange : MonoBehaviour
{
    //RGBAは0~1の間で設定する
    [SerializeField] Color color_Enter = new Color(0.542f,1f,0.887f,1f);
    [SerializeField] Color color_Exit = new Color(1f,1f,1f,1f);
    
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
        this.GetComponent<Image>().color = color_Enter;
    }

    public void OnPointExit()
    {
        this.GetComponent<Image>().color = color_Exit;
    }
}
