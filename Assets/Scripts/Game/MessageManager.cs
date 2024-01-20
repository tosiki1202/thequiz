using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using TMPro;
using Photon.Pun;

public class MessageManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private int NowQuestionIndex;
    [SerializeField] private int DELAYSHOWMS;
    private int correct = 0;
    public static MessageManager instance;
    public TextMeshProUGUI correctAnsNum_1;
    public TextMeshProUGUI correctAnsNum_2;
    public TextMeshProUGUI playerName_1;
    public TextMeshProUGUI playerName_2;
    public Image playerIsCorrectImage_1;
    public Image playerIsCorrectImage_2;
    public Sprite maru;
    public Sprite batu;
    public Sprite none_sprite;
    public TextMeshProUGUI qNumText;
    public TextMeshProUGUI sentence_box;
    public TextMeshProUGUI sel_1_box;
    public TextMeshProUGUI sel_2_box;
    public TextMeshProUGUI sel_3_box;
    public TextMeshProUGUI sel_4_box;
    public SetButton setButton;
    public CancellationTokenSource cancelToken;
    private UniTask task;
    public Question[] merged_question = new Question[MessageGeter.question.Length * GeneUIManager.allPlayerInfo.Count];
    public Data[] merged_data = new Data[MessageGeter.question.Length * GeneUIManager.allPlayerInfo.Count];
    private bool is_correct;

    AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        GeneUIManager.player.GetComponent<PlayerController>().ready = false;
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i=0; i<GeneUIManager.allPlayerInfo.Count; i++)
        {
            for (int j=0; j<MessageGeter.question.Length; j++)
            {
                merged_question[i*MessageGeter.question.Length + j] = GeneUIManager.allPlayerInfo[i].my_question[j];
                //merged_data[i*MessageGeter.question.Length + j] = GeneUIManager.allPlayerInfo[i].my_data[j];
            }
        }
    }

    void Update()
    {   
        playerName_1.text = GeneUIManager.allPlayerInfo[0].name;
        correctAnsNum_1.text = "正答数：" + GeneUIManager.allPlayerInfo[0].correct + "/" + merged_question.Length;
        if (!GeneUIManager.allPlayerInfo[0].is_answered) playerIsCorrectImage_1.sprite = none_sprite;
        else if (GeneUIManager.allPlayerInfo[0].my_data[NowQuestionIndex].q_correct) playerIsCorrectImage_1.sprite = maru;
        else if (!GeneUIManager.allPlayerInfo[0].my_data[NowQuestionIndex].q_correct) playerIsCorrectImage_1.sprite = batu;
        
        if (GeneUIManager.allPlayerInfo.Count == 1) return;
        playerName_2.text = GeneUIManager.allPlayerInfo[1].name;
        correctAnsNum_2.text = "正答数：" + GeneUIManager.allPlayerInfo[1].correct + "/" + merged_question.Length;
        if (!GeneUIManager.allPlayerInfo[1].is_answered) playerIsCorrectImage_2.sprite = none_sprite;
        else if (GeneUIManager.allPlayerInfo[1].my_data[NowQuestionIndex].q_correct) playerIsCorrectImage_2.sprite = maru;
        else if (!GeneUIManager.allPlayerInfo[1].my_data[NowQuestionIndex].q_correct) playerIsCorrectImage_2.sprite = batu;

    }

    public async UniTask Q_Displaycontrol()
    {
        //UniTaskの関数をキャンセル可能にするために、トークンを作る
        cancelToken = new CancellationTokenSource();  
        CancellationToken token = cancelToken.Token;
        //audioSource.PlayOneShot(audioSource.clip);
        await Show(qNumText, "問題." + (NowQuestionIndex + 1), token);
        await UniTask.Delay(700);
        sel_1_box.text = merged_question[NowQuestionIndex].sel_1;
        sel_2_box.text = merged_question[NowQuestionIndex].sel_2;
        sel_3_box.text = merged_question[NowQuestionIndex].sel_3;
        sel_4_box.text = merged_question[NowQuestionIndex].sel_4;
        setButton.timer.GoTimer();
        setButton.InitButton();

        //Show()の実行状態を、UniTask型変数に格納
        task = Show(sentence_box, merged_question[NowQuestionIndex].sentence, token);   
    }

    [PunRPC]
    public async UniTask NextQuestion(){
        setButton.ButtonNotAct();
        GeneUIManager.player.GetComponent<PlayerController>().is_answered = false;
        GeneUIManager.player.GetComponent<PlayerController>().is_stored = false;
        
        // if(StoreButtonData.data[NowQuestionIndex].q_correct == true){
        //     correct += 1;
        // }
        //correctAnsNum.text = "正答数：" + correct + "/" + merged_question.Length;

        NowQuestionIndex++;
        if (NowQuestionIndex+1 > merged_question.Length)
        {
            Debug.Log("問題終了");
            await UniTask.Delay(1500);
            SceneManager.LoadScene("ResultScene");
            return;
        }
        await Q_Displaycontrol();
    }

    //表示上限数を1ずつ上げることで、いい感じに文字表示する関数
    private async UniTask Show(TextMeshProUGUI _box, string _text, CancellationToken token)
    {
        for (int i=0; i<_text.Length; i++)
        {
            _box.maxVisibleCharacters = i;
            _box.text = _text;
            await UniTask.Delay(DELAYSHOWMS);
            //Show()のキャンセルがリクエストされた時に、_boxの最大文字数表示を0にする
            if (token.IsCancellationRequested)
            {
                _box.maxVisibleCharacters = 0;
                return;
            }  
        }
        _box.maxVisibleCharacters = _text.Length;
    }

    // 文章が入るボックスを綺麗にする関数
    public void ClearQuizSet()
    {
        //Show()が完了していないときに呼び出されたら、トークンを用いてそのShow()をキャンセルする
        var isCompleted = task.GetAwaiter().IsCompleted;
        if (!isCompleted)
        {
            cancelToken.Cancel();
        }
        qNumText.text = null;
        sentence_box.text = null;
        sel_1_box.text = null;
        sel_2_box.text = null;
        sel_3_box.text = null;
        sel_4_box.text = null;
    }

    public void SetQuestionIndex(int idx)
    {
        NowQuestionIndex = idx;
    }
    public int GetQuestionIndex()
    {
        return NowQuestionIndex;
    }
}
