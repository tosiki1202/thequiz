using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ボタンを押下した後TruePanelにアタッチしたパネルを表示し
// FalsePanelにアタッチしたパネルを非表示にする
public class PanelChanger : MonoBehaviour
{
    public Button button;
    public GameObject TruePanel;
    public GameObject FalsePanel;

    void Start()
    {
        button.onClick.AddListener(Transit);
    }
    public void Transit()
    {
        TruePanel.SetActive(true);
        FalsePanel.SetActive(false);
    }
}
