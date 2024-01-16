//SetButtonで取得したidxとtimeを格納する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtonData : MonoBehaviour{
    private int i;
    public struct Data{
        public int q_num;
        public int q_sel;
        public float q_time;
        public bool q_correct;
    }

    public static Data[] data = new Data[MessageGeter.question.Length*2];//問題数に応じて変更
    public void Comparison(int i){
        if(MessageManager.instance.merged_question[i].answer_index == data[i].q_sel){
            data[i].q_correct = true;
        }else{
            data[i].q_correct = false;
        }
    }
    public void DataSave(int idx, float timeStop){
        i = MessageManager.instance.GetQuestionIndex();
        if(i<MessageManager.instance.merged_question.Length){
            data[i].q_num = i + 1;
            data[i].q_sel = idx;
            data[i].q_time = timeStop;
            Comparison(i);
        }
    }
}