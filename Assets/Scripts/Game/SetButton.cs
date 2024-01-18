//押したボタンのidxと押すまでにかかった時間を取得する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
using Photon.Pun;
using Cysharp.Threading.Tasks;
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
    public GameObject answeredPanel;
    public Image answeredImage;
    public Sprite maru;
    public Sprite batu;

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
        if (!GeneUIManager.player.GetComponent<PlayerController>().is_answered)
        {
            timeUp = timer.GetTimeUp();
            TimeRing.fillAmount = 1.0f - (timeUp / timer.GetTIMELIMIT());
            time = 10.0f - timeUp;
            timerText.text = time.ToString("f0");
        }
        
        if (click || timer.GetTimeUp() > timer.GetTIMELIMIT())
        {
            timeStop = float.Parse(timeUp.ToString("f2"));
            StoreInfo();
            click = false;
            timer.InitTimer();

            if (GeneUIManager.player.GetComponent<PlayerController>().my_data[messageManager.GetQuestionIndex()].q_correct)
            {
                answeredImage.sprite = maru;
            }
            else
            {
                answeredImage.sprite = batu;
            }
            answeredPanel.SetActive(true);
            await UniTask.Delay(700);
            answeredPanel.SetActive(false);
            GeneUIManager.player.GetComponent<PlayerController>().is_answered = true;
        }

        for (int i=0; i<GeneUIManager.allPlayerInfo.Count; i++)
        {
            if (!GeneUIManager.allPlayerInfo[i].is_answered) return;
        }
        
        messageManager.photonView.RPC("NextQuestion",RpcTarget.All);
        
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