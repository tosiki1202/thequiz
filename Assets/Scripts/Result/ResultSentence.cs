using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultSentence : MonoBehaviour
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
    void Start()
    {
        ExitButton.onClick.AddListener(Transit);
    }
    public void SentenceDisplay(int selectNumber){
        id.text = StoreButtonData.data[selectNumber].q_num.ToString() + "問目";
        sel_Player.text = "選択：" + StoreButtonData.data[selectNumber].q_sel.ToString();
        sentence.text = "問題\n" + MessageManager.merged_question[selectNumber].sentence;
        sel_1.text = "選択肢１\n" + MessageManager.merged_question[selectNumber].sel_1;
        sel_2.text = "選択肢２\n" + MessageManager.merged_question[selectNumber].sel_2;
        sel_3.text = "選択肢３\n" + MessageManager.merged_question[selectNumber].sel_3;
        sel_4.text = "選択肢４\n" + MessageManager.merged_question[selectNumber].sel_4;
        if(MessageManager.merged_question[selectNumber].answer_index == 1){
            ans.text = "解答：" + MessageManager.merged_question[selectNumber].answer_index;
        }
        else if(MessageManager.merged_question[selectNumber].answer_index == 2){
            ans.text = "解答：" + MessageManager.merged_question[selectNumber].answer_index;
        }
        else if(MessageManager.merged_question[selectNumber].answer_index == 3){
            ans.text = "解答：" + MessageManager.merged_question[selectNumber].answer_index;
        }
        else if(MessageManager.merged_question[selectNumber].answer_index == 4){
            ans.text = "解答：" + MessageManager.merged_question[selectNumber].answer_index;
        }
    }
    public void Transit()
    {
        TruePanel.SetActive(true);
        FalsePanel.SetActive(false);
    }
}
