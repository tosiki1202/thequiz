using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class ResultSentenceMulti : MonoBehaviour
{
    public TextMeshProUGUI id;
    public TextMeshProUGUI ans;
    public TextMeshProUGUI sel_Player;
    public TextMeshProUGUI sentence;
    public TextMeshProUGUI sel_1;
    public TextMeshProUGUI sel_2;
    public TextMeshProUGUI sel_3;
    public TextMeshProUGUI sel_4;
    public Button ExitButton;
    public GameObject TruePanel;
    public GameObject FalsePanel;
    public Question[] merged_question = new Question[MessageGeter.question.Length * GeneUIManager.allPlayerInfo.Count];
    void Start()
    {
        ExitButton.onClick.AddListener(Transit);
        for (int i=0; i<GeneUIManager.allPlayerInfo.Count; i++)
        {
            for (int j=0; j<MessageGeter.question.Length; j++)
            {
                merged_question[i*MessageGeter.question.Length + j] = GeneUIManager.allPlayerInfo[i].my_question[j];
                //merged_data[i*MessageGeter.question.Length + j] = GeneUIManager.allPlayerInfo[i].my_data[j];
            }
        }
    }
    public void SentenceDisplay(int selectNumber){
        id.text = StoreButtonData.data[selectNumber].q_num.ToString() + "問目";
        sel_Player.text = "選択：" + GeneUIManager.allPlayerInfo[0].my_data[selectNumber].q_sel.ToString();
        sentence.text = "問題\n" + merged_question[selectNumber].sentence;
        sel_1.text = "選択肢１\n" + merged_question[selectNumber].sel_1;
        sel_2.text = "選択肢２\n" + merged_question[selectNumber].sel_2;
        sel_3.text = "選択肢３\n" + merged_question[selectNumber].sel_3;
        sel_4.text = "選択肢４\n" + merged_question[selectNumber].sel_4;
        if(merged_question[selectNumber].answer_index == 1){
            ans.text = "解答：" + merged_question[selectNumber].sel_1;
        }
        else if(merged_question[selectNumber].answer_index == 2){
            ans.text = "解答：" + merged_question[selectNumber].sel_2;
        }
        else if(merged_question[selectNumber].answer_index == 3){
            ans.text = "解答：" + merged_question[selectNumber].sel_3;
        }
        else if(merged_question[selectNumber].answer_index == 4){
            ans.text = "解答：" + merged_question[selectNumber].sel_4;
        }
    }
    public void Transit()
    {
        TruePanel.SetActive(true);
        FalsePanel.SetActive(false);
    }
}
