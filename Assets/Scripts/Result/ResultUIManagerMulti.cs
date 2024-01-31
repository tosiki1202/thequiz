using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.SceneManagement;

public class ResultUIManagerMulti : MonoBehaviour
{
    //プレハブ化したものを設定する
    public ResultPrefabMulti originalResultPrefabMulti;
    //プレハブ化したものが、生成される親オブジェクトを決める
    public ResultSentence resultSentence;
    public GameObject qContent;
    public GameObject questionPanel;
    public GameObject qDataPanel;
    public TextMeshProUGUI janru;
    public TextMeshProUGUI info1;
    public TextMeshProUGUI info2;
    public TextMeshProUGUI correctQNum;
    private float time1 = 0;
    private float time2 = 0;
    public GameObject winOrLoseText;
    public GameObject errorPanel;
    public TextMeshProUGUI errorText;

    public void CloseErrorPanel()
    {
        SceneManager.LoadScene("Photon_experimentScene");
    }
    void Start()
    {
        errorPanel.SetActive(false);
        if (PhotonNetwork.CurrentRoom.MaxPlayers != GeneUIManager.allPlayerInfo.Count)
        {
            errorText.text = "既に完了したゲームです。";
            errorPanel.SetActive(true);
        }

        for (int i=0; i<MessageGeter.question.Length * GeneUIManager.allPlayerInfo.Count; i++)
        {
            int _count = i;
            //プレハブから、実体を一つ作る
            ResultPrefabMulti newPrefab = Instantiate(originalResultPrefabMulti);
            //ボタンの設定をする
            newPrefab.detailButton.onClick.AddListener(() => Transit());
            newPrefab.detailButton.onClick.AddListener(() => resultSentence.SentenceDisplay(_count));
            //プレハブに値を入れる
            newPrefab.RegisterResultPrefabMulti(StoreButtonData.data[i].q_num,
                                           GeneUIManager.allPlayerInfo[0].my_data[i].q_correct,
                                           GeneUIManager.allPlayerInfo[1].my_data[i].q_correct,
                                           GeneUIManager.allPlayerInfo[0].my_data[i].q_time,
                                           GeneUIManager.allPlayerInfo[1].my_data[i].q_time);

            //親オブジェクトを設定する
            newPrefab.transform.SetParent(qContent.transform,false);
            time1 += GeneUIManager.allPlayerInfo[0].my_data[i].q_time;
            time2 += GeneUIManager.allPlayerInfo[1].my_data[i].q_time;
                    janru.text = "ジャンル：" + GeneUIManager.allPlayerInfo[0].jyanru + "　" + GeneUIManager.allPlayerInfo[1].jyanru;
            info1.text = GeneUIManager.allPlayerInfo[0].name + "\n正答数：" + GeneUIManager.allPlayerInfo[0].correct;
            info2.text = GeneUIManager.allPlayerInfo[1].name + "\n正答数：" + GeneUIManager.allPlayerInfo[1].correct;
        }

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
