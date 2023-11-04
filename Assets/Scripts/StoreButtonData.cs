//SetButtonで取得したidxとtimeを格納する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtonData : MonoBehaviour{
    [SerializeField] private int MAXQUESTIONINDEX = 3;
    public MessageManager messageManager;
    private int i;
    public struct Data{
        public int q_num;
        public int q_sel;
        public float q_time;
        public bool q_correct;
        public string q_answer;
    }

    public static Data[] data = new Data[3];//問題数に応じて変更
    public void AnswerText(int i){
        if(MessageGeter.question[i].answer_index == 1){
            data[i].q_answer = MessageGeter.question[i].sel_1;
        }
        else if(MessageGeter.question[i].answer_index == 2){
            data[i].q_answer = MessageGeter.question[i].sel_2;
        }
        else if(MessageGeter.question[i].answer_index == 3){
            data[i].q_answer = MessageGeter.question[i].sel_3;
        }
        else if(MessageGeter.question[i].answer_index == 4){
            data[i].q_answer = MessageGeter.question[i].sel_4;
        }
    }
    public void Comparison(int i){
        if(MessageGeter.question[i].answer_index == data[i].q_sel){
            data[i].q_correct = true;
        }else{
            data[i].q_correct = false;
        }
    }
    public void DataSave(int idx, float timeStop){
        i = messageManager.GetQuestionIndex();
        if(i<MAXQUESTIONINDEX){
            data[i].q_num = i + 1;
            data[i].q_sel = idx;
            data[i].q_time = timeStop;
            AnswerText(i);
            Comparison(i);
            Debug.Log(data[i].q_answer + "←答えの文　正解か→" + data[i].q_correct.ToString());
            //for (int j = 0; j < i + 1; j++)
            //{
            //    Debug.Log("Data at index " + j + ": idx = " + data[j].id + ", timeStop = " + data[j].time);
            //}
        }
        else{
            Debug.Log("error");
        }
    }
}