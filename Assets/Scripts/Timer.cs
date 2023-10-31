//カウントアップとカウントダウンの時間を計測
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeUp;//カウントアップ用
    public float timeDown; //カウントダウン用
    private bool go;

    void Start(){
        InitTimer();
    }
    void Update()
    {
        if(go)
        {
            TimerSet();
        }
    }
    //InitTimer()
    //カウントダウンの初期値変更部分(timeDown)
    private void InitTimer()
    {
        go = false;
        timeDown = 10;
        timeUp = 0;
    }
    //TimerSet()
    //カウントアップの最大値変更部分(timeUp)
    private void TimerSet()
    {
        if (timeDown > 0)
        {
            timeDown -= Time.deltaTime;
        }
        else
        {
            timeDown = 0;
        }
        if (timeUp < 10)
        {
            timeUp += Time.deltaTime;
        }
        else
        {
            timeUp = 10;
        }
        timeUp = Mathf.Max(timeUp, 0) % 60;
        timeDown = Mathf.Max(timeDown, 0) % 60;
    }
    public float GetTimeUp()
    {
        return timeUp;
    }
    public float GetTimeDown()
    {
        return timeDown;
    }
    public void GoTimer()
    {
        InitTimer();
        go=true;
    }
}
