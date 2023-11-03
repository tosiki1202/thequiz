//SetButtonで取得したidxとtimeを格納する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtonData : MonoBehaviour{
    [SerializeField] private int MAXQUESTIONINDEX = 3;
    public struct Data{
        public int q_num;
        public int q_sel;
        public float q_time;
    }

    public static Data[] data = new Data[3];//問題数に応じて変更
    public MessageManager messageManager;
    private int i;
    public void DataSave(int idx, float timeStop){
        i = messageManager.GetQuestionIndex();
        if(i<MAXQUESTIONINDEX){
            data[i].q_num = i + 1;
            data[i].q_sel = idx;
            data[i].q_time = timeStop;
            //Debug.Log(i + "回目");
            //for (int j = 0; j < i + 1; j++)
            //{
            //    Debug.Log("Data at index " + j + ": idx = " + data[j].id + ", timeStop = " + data[j].time);
            //}
            i += 1;
        }
        else{
            Debug.Log("error");
        }
    }
}