using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultUIManager : MonoBehaviour
{
    //プレハブ化したものを設定する
    public ResultPrefab originalResultPrefab;
    //プレハブ化したものが、生成される親オブジェクトを決める
    public ResultSentence resultSentence;
    public GameObject qContent;
    public GameObject questionPanel;
    public GameObject qDataPanel;
    public TextMeshProUGUI genre;
    public TextMeshProUGUI correctQNum;
    private int correctNum = 0;
    public GameObject winOrLoseText;
    void Start()
    {
        if (GeneUIManager.allPlayerInfo.Count == 1)
        {
            winOrLoseText.SetActive(false);
        }

        for (int i=0; i<GeneUIManager.player.GetComponent<PlayerController>().my_data.Length; i++)
        {
            int _count = i;
            if(StoreButtonData.data[i].q_correct == true){
                correctNum += 1;
            }
            //プレハブから、実体を一つ作る
            ResultPrefab newPrefab = Instantiate(originalResultPrefab);
            //ボタンの設定をする
            newPrefab.detailButton.onClick.AddListener(() => Transit());
            newPrefab.detailButton.onClick.AddListener(() => resultSentence.SentenceDisplay(_count));
            //プレハブに値を入れる
            newPrefab.RegisterResultPrefab(StoreButtonData.data[i].q_num,
                                           StoreButtonData.data[i].q_correct,
                                           StoreButtonData.data[i].q_time);

            //親オブジェクトを設定する
            newPrefab.transform.SetParent(qContent.transform,false);
        }
        string s = "";
        for (int i=0; i<GeneUIManager.allPlayerInfo.Count; i++)
        {
            s += GeneUIManager.allPlayerInfo[i].jyanru;
            s += "　";
        }
        genre.text = "ジャンル：" + s;
        correctQNum.text = correctNum.ToString() + "問正解";

        int other_index=0;
        for (int i=0; i<GeneUIManager.allPlayerInfo.Count; i++)
        {
            if (GeneUIManager.player.GetComponent<PlayerController>().name != GeneUIManager.allPlayerInfo[i].name)
            {
                other_index = i;
                break;
            }
        }
        if (GeneUIManager.player.GetComponent<PlayerController>().point > GeneUIManager.allPlayerInfo[other_index].point)
        {
            winOrLoseText.GetComponent<TextMeshProUGUI>().text = "You Win!";   
            winOrLoseText.GetComponent<TextMeshProUGUI>().color = new Color(0.542f,1f,0.887f,1f);
        }
        else if (GeneUIManager.player.GetComponent<PlayerController>().point < GeneUIManager.allPlayerInfo[other_index].point)
        {
            winOrLoseText.GetComponent<TextMeshProUGUI>().text = "You Lose...";   
            winOrLoseText.GetComponent<TextMeshProUGUI>().color = new Color(0.542f,1f,0.887f,1f);
        }
        else
        {
            winOrLoseText.GetComponent<TextMeshProUGUI>().text = "DRAW";
            winOrLoseText.GetComponent<TextMeshProUGUI>().color = new Color(0.542f,1f,0.887f,1f);
        }
    }
    public void Transit()
    {
        qDataPanel.SetActive(false);
        questionPanel.SetActive(true);
    }
}
