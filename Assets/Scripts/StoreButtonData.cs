//SetButtonで取得したidxとtimeを格納する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Data
{
    public int id;
    public float time;

    public Data(int id, float time)
    {
        this.id = id;
        this.time = time;
    }
}

public class StoreButtonData : MonoBehaviour{
    public List<Data> data;
    void Start()
    {
        data = new List<Data>();
    }
    public void DataSave(int idx, float timeStop){
        Data dt = new Data();
        dt.id = idx;
        dt.time = timeStop;
        data.Add(dt);
        for (int i = 0; i < data.Count; i++)
        {
            //Debug.Log("Data at index " + i + ": idx = " + data[i].id + ", timeStop = " + data[i].time);
        }
    }
}