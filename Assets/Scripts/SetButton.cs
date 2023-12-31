//押したボタンのidxと押すまでにかかった時間を取得する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
public class SetButton : MonoBehaviour
{
    public Timer timer;
    public StoreButtonData data;
    public MessageManager messageManager;
    public Image TimeRing;
    public TextMeshProUGUI timerText;
    private float time;
    private float timeUp;
    private float timeStop; 
    private int idx = 0;
    private bool click = false;
    private List<Button> children = new List<Button>();

    //Start()の実行前に呼び出される初期化関数
    void Awake()
    {
        messageManager.ClearQuizSet();
        //子オブジェクトのButtonコンポーネントをListに格納
        for (int i=0; i<gameObject.transform.childCount; i++)
        {
            children.Add(gameObject.transform.GetChild(i).GetComponent<Button>());
        }
    }

    async void Start(){
        InitButton();
        ButtonNotAct();
        await messageManager.Q_Displaycontrol();
    }
    

    async void Update()
    {
        timeUp = timer.GetTimeUp();
        TimeRing.fillAmount = 1.0f - (timeUp / timer.GetTIMELIMIT());
        time = 10.0f - timeUp;
        timerText.text = time.ToString("f0");
        if (click || timer.GetTimeUp() > timer.GetTIMELIMIT())
        {
            timeStop = float.Parse(timeUp.ToString("f2"));
            StoreInfo();
            click = false;
            timer.InitTimer();
            await messageManager.NextQuestion();
        }
    }

    public void InitButton(){
        idx = 0;
        click = false;
        ButtonAct();
    }

    public void ButtonAct(){
        for (int i=0; i<children.Count; i++)
        {
            children[i].interactable = true;
        }
    }
    
    public void ButtonNotAct(){
        for (int i=0; i<children.Count; i++)
        {
            children[i].interactable = false;
        }    
    }

    //OnClickに追加
    public void ClickButton(string button)
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
        click = true;
        messageManager.ClearQuizSet();
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