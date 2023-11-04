//カウントアップタイマーとカウントダウンタイマー

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeUp;
    public float timeDown; 
    private bool go;
    [SerializeField] private float TIMELIMIT;

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
    public void InitTimer()
    {
        go = false;
        timeDown = 10;
        timeUp = 0;
    }

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
        if (timeUp < TIMELIMIT)
        {
            timeUp += Time.deltaTime;
        }
        else
        {
            timeUp = TIMELIMIT;
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
    public float GetTIMELIMIT()
    {
        return TIMELIMIT;
    }
}

