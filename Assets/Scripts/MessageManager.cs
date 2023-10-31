using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Timer timer;
    public MessageGeter messageGeter;
    public StoreButtonData storeButtonData;
    public TextMeshProUGUI sentence_box;
    public TextMeshProUGUI sel_1_box;
    public TextMeshProUGUI sel_2_box;
    public TextMeshProUGUI sel_3_box;
    public TextMeshProUGUI sel_4_box;
    public TextMeshProUGUI answer_index_box;
    public TextMeshProUGUI selected_index_box;
    public SetButton setButton1;
    public SetButton setButton2;
    public SetButton setButton3;
    public SetButton setButton4;
    [SerializeField] private int NowQuestionIndex;

    void Start()
    {
        NowQuestionIndex = 0;   
    }

    // Update is called once per frame
    public void Q_Displaycontrol()
    {
        sentence_box.text = messageGeter.question[NowQuestionIndex].sentence;
        sel_1_box.text = messageGeter.question[NowQuestionIndex].sel_1;
        sel_2_box.text = messageGeter.question[NowQuestionIndex].sel_2;
        sel_3_box.text = messageGeter.question[NowQuestionIndex].sel_3;
        sel_4_box.text = messageGeter.question[NowQuestionIndex].sel_4;
        answer_index_box.text = messageGeter.question[NowQuestionIndex].answer_index.ToString("0");
        //selected_index_box.text = storeButtonData.data[NowQuestionIndex].id.ToString("0");
        timer.GoTimer();
    }
    //NextQuestion()
    //if(i = 問題数)でシーン遷移を追加してもよい
    //問題表示前にunitask等使用して正解不正解表示していてもよい
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
    //public void SetQuestionIndex(int idx)
    //{
    //    NowQuestionIndex = idx;
    //}
    public int GetQuestionIndex()
    {
        return NowQuestionIndex;
    }
}
