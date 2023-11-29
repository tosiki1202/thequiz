using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    public void ActivatePanel()
    {
        panel.SetActive(true);
    }

    public void InactivatePanel()
    {
        panel.SetActive(false);
    }
}
