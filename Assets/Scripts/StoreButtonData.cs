//構造体に格納する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StoreButtonData : MonoBehaviour{
    public struct Data{
        public int id;
        public float time;
    }
    public Data[] data = new Data[3];//問題数に応じて変更
    public MessageManager messageManager;
    private int i;
    public void DataSave(int idx, float timeStop){
        i = messageManager.GetQuestionIndex();
        if(i<3){
            data[i].id = idx;
            data[i].time = timeStop;
            Debug.Log(i + "回目");
            for (int j = 0; j < i + 1; j++)
            {
                Debug.Log("Data at index " + j + ": idx = " + data[j].id + ", timeStop = " + data[j].time);
            }
            i += 1;
        }
        else{
            Debug.Log("error");
        }
    }
}