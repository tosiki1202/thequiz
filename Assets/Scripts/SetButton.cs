//押したボタンのidxと押すまでにかかった時間を取得する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
public class SetButton : MonoBehaviour
{
    public Timer timer;
    public StoreButtonData data;
    public MessageManager messageManager;
    private float timeUp;
    private float timeStop; 
    private int idx;
    private bool click;
 

    void Start(){
        InitButton();
        ButtonNotAct();
    }
    async void Update()
    {
        //Debug.Log("Update method called");
        timeUp = timer.GetTimeUp();
        //Debug.Log(timeUp);
        if(timeUp == 10.0 && !click){
            //Debug.Log("Condition met");
            timeStop = 10.0f;//タイマーの上限値に変更
            StoreInfo();
            await messageManager.NextQuestion();
            InitButton();
            click = true;
        }
    }
    public void InitButton(){
        idx = 0;
        click = false;
        ButtonAct();
    }
    private void ButtonAct(){
        MyCanvas.SetActive("Button1", true);
        MyCanvas.SetActive("Button2", true);
        MyCanvas.SetActive("Button3", true);
        MyCanvas.SetActive("Button4", true);
    }
    private void ButtonNotAct(){
        MyCanvas.SetActive("Button1", false);
        MyCanvas.SetActive("Button2", false);
        MyCanvas.SetActive("Button3", false);
        MyCanvas.SetActive("Button4", false);        
    }

    //OnClickに追加
    public async void ClickButton(string button)
    {
        //Debug.Log("Button clicked: " + button);
        switch (button)
        {
            case "button1":
                idx = 1;
                break;
            case "button2":
                idx = 2;
                break;
            case "button3":
                idx = 3;
                break;
            case "button4":
                idx = 4;
                break;
            default:
                break;
        }
        timeStop = timeUp;
        timeStop = Mathf.Max(timeStop, 0) % 60;
        click = true;
        StoreInfo();
        ButtonNotAct();
        await messageManager.NextQuestion();
    }
    public void StoreInfo(){
        data.DataSave(idx, timeStop);
    }
    public float GetTimeStop(){
        return timeStop;
    }
    public int GetIdx(){
        return idx;
    }
}