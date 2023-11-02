using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private int MAXQUESTIONINDEX;
    public Timer timer;
    public MessageGeter messageGeter;
    public SetButton setButton1;
    public SetButton setButton2;
    public SetButton setButton3;
    public SetButton setButton4;
    public StoreButtonData storeButtonData;
    public TextMeshProUGUI sentence_box;
    public TextMeshProUGUI sel_1_box;
    public TextMeshProUGUI sel_2_box;
    public TextMeshProUGUI sel_3_box;
    public TextMeshProUGUI sel_4_box;
    public TextMeshProUGUI answer_index_box;
    public TextMeshProUGUI selected_index_box;

    [SerializeField] private int NowQuestionIndex;

    public void Q_Displaycontrol()
    {
        sentence_box.text = messageGeter.question[NowQuestionIndex].sentence;
        sel_1_box.text = messageGeter.question[NowQuestionIndex].sel_1;
        sel_2_box.text = messageGeter.question[NowQuestionIndex].sel_2;
        sel_3_box.text = messageGeter.question[NowQuestionIndex].sel_3;
        sel_4_box.text = messageGeter.question[NowQuestionIndex].sel_4;
        answer_index_box.text = messageGeter.question[NowQuestionIndex].answer_index.ToString("0");
        //if (storeButtonData.data.Count == 0) return;
        //selected_index_box.text = storeButtonData.data[NowQuestionIndex].id.ToString("0");
        timer.GoTimer();
    }
    public void NextQuestion(){
        NowQuestionIndex+=1;
        if(NowQuestionIndex < 3){
            Q_Displaycontrol();
            setButton1.InitButton();
            setButton2.InitButton();
            setButton3.InitButton();
            setButton4.InitButton();
        }
        else{
            Debug.Log("error");
        }
    }
    public void SetQuestionIndex(int idx)
    {
        NowQuestionIndex = idx;
    }
    public int GetQuestionIndex()
    {
        return NowQuestionIndex;
    }
    public int GetMAXQUESTIONINDEX()
    {
        return MAXQUESTIONINDEX;
    }
}
