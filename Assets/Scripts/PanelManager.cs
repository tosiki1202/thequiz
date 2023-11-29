using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }
    public void ActivatePanel()
    {
        panel.SetActive(true);
    }

    public void InactivatePanel()
    {
        panel.SetActive(false);
    }
}
