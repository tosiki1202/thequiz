using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultUIManager : MonoBehaviour
{
    public ResultSentence resultSentence;
    [SerializeField] private int SELECTQUESTIONINDEX;
    public Button SentenceViewButton;
    public GameObject TruePanel;
    public GameObject FalsePanel;
    void Start()
    {
        SentenceViewButton.onClick.AddListener(Transit);
    }
    public void Transit()
    {
        resultSentence.SentenceDisplay(SELECTQUESTIONINDEX);
        TruePanel.SetActive(true);
        FalsePanel.SetActive(false);
    }
}
