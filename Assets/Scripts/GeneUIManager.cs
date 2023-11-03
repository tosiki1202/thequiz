using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneUIManager : MonoBehaviour
{
    public static GeneUIManager instance;
    [SerializeField] TextMeshProUGUI GeneratingText;
    [SerializeField] GameObject GeneratePanel;

    private void Awake()
    {
        instance = this;
    }

    public void CloseGeneUI()
    {
        GeneratePanel.SetActive(false);
    }

    public void GeneratingUIDisplay()
    {
        CloseGeneUI();
        GeneratePanel.SetActive(true);

    }

    public void SetGeneratingText(string _text)
    {
        GeneratingText.text = _text;
    }
}
