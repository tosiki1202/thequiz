//SetButtonで取得したidxとtimeを格納する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StoreButtonData : MonoBehaviour{
    private int i;

    public static Data[] data = new Data[MessageGeter.question.Length * GeneUIManager.allPlayerInfo.Count];//問題数に応じて変更
    public void Comparison(int i){
        if(MessageManager.merged_question[i].answer_index == data[i].q_sel){
            data[i].q_correct = true;
            GeneUIManager.player.GetComponent<PlayerController>().correct++;
        }else{
            data[i].q_correct = false;
        }
    }
    public void DataSave(int idx, float timeStop){
        i = MessageManager.instance.GetQuestionIndex();
        if(i<MessageManager.merged_question.Length){
            data[i].q_num = i + 1;
            data[i].q_sel = idx;
            data[i].q_time = timeStop;
            Comparison(i);
        }
    }
}