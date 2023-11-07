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
    void Start()
    {
        genre.text = MessageGeter.genre;
        for (int i=0; i<MessageGeter.question.Length; i++)
        {
            int _count = i;
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
            newPrefab.transform.SetParent(qContent.transform);
        }
    }
    public void Transit()
    {
        qDataPanel.SetActive(false);
        questionPanel.SetActive(true);
    }
}
