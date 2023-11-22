using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject button1;
    [SerializeField] GameObject button2;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }
    public void ActivatePanel()
    {
        panel.SetActive(true);
        button1.SetActive(false);
        button2.SetActive(true);
    }

    public void InactivatePanel()
    {
        panel.SetActive(false);
        button1.SetActive(true);
        button2.SetActive(false);
    }
}
