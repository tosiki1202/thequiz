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
using UnityEngine.SceneManagement;
public class SetButton : MonoBehaviourPunCallbacks
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
    public GameObject answerText;
    private bool disconnectToClient = false;
    public GameObject errorPanel;
    public TextMeshProUGUI errorText;
    public GameObject waitingText;

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
        answerText.GetComponent<TextMeshProUGUI>().maxVisibleCharacters = 0;
        await messageManager.Q_Displaycontrol();
    }
    

    async void Update()
    {
        if (disconnectToClient)
        {
            return;
        }
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
            GeneUIManager.player.GetComponent<PlayerController>().is_answered = true;

            if (GeneUIManager.player.GetComponent<PlayerController>().my_data[messageManager.GetQuestionIndex()].q_correct)
            {
                answeredImage.sprite = maru;
            }
            else
            {
                answeredImage.sprite = batu;
            }
            answeredPanel.SetActive(true);
            await UniTask.Delay(1200);
            answeredPanel.SetActive(false);
            await Show(answerText.GetComponent<TextMeshProUGUI>(),"正解: "+messageManager.merged_question[messageManager.GetQuestionIndex()].answer_index);
            GeneUIManager.player.GetComponent<PlayerController>().is_stored = true;
            waitingText.SetActive(true);
        }

        for (int i=0; i<GeneUIManager.allPlayerInfo.Count; i++)
        {
            if (!GeneUIManager.allPlayerInfo[i].is_stored) return;
        }
        //全員のis_answeredがtrueになってからn秒後にRPC
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

    private async UniTask Show(TextMeshProUGUI _box, string _text)
    {
        for (int i=0; i<_text.Length+1; i++)
        {
            _box.maxVisibleCharacters = i;
            _box.text = _text;
            await UniTask.Delay(50);
        }
        await UniTask.Delay(1800);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        disconnectToClient = true;
        errorPanel.SetActive(true);
        errorText.text = "他のプレイヤーとの接続が切断されました。ページを再読み込みしてください。";
    }
}