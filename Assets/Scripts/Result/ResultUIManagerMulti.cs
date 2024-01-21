using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

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
    void Start()
    {
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
            newPrefab.transform.SetParent(qContent.transform);
            time1 += GeneUIManager.allPlayerInfo[0].my_data[i].q_time;
            time2 += GeneUIManager.allPlayerInfo[1].my_data[i].q_time;
                    janru.text = "ジャンル：" + GeneUIManager.allPlayerInfo[0].jyanru +GeneUIManager.allPlayerInfo[0].jyanru;
        info1.text = GeneUIManager.allPlayerInfo[0].name + "\n正答数：" + GeneUIManager.allPlayerInfo[0].correct + "時間：" + time1;
        info2.text = GeneUIManager.allPlayerInfo[1].name + "\n正答数：" + GeneUIManager.allPlayerInfo[1].correct + "時間：" + time2;
        }
    }
    public void Transit()
    {
        qDataPanel.SetActive(false);
        questionPanel.SetActive(true);
    }
}
