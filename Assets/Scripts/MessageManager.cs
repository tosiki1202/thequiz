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

public class MessageManager : MonoBehaviour
{
    [SerializeField] private int NowQuestionIndex;
    [SerializeField] private int DELAYSHOWMS;
    private int correct = 0;
    public TextMeshProUGUI correctAnsNum;
    public TextMeshProUGUI qNumText;
    public TextMeshProUGUI sentence_box;
    public TextMeshProUGUI sel_1_box;
    public TextMeshProUGUI sel_2_box;
    public TextMeshProUGUI sel_3_box;
    public TextMeshProUGUI sel_4_box;
    public SetButton setButton;
    public CancellationTokenSource cancelToken;
    private UniTask task;
    
    public Image maru;
    public Image batu;
    public Sprite Spritemaru;
    public Sprite Spritebatu;
    public TextMeshProUGUI answer;
    [SerializeField] GameObject marubatupanel;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maru.sprite = Spritemaru;
        batu.sprite = Spritebatu;
        maru.enabled = false;
        batu.enabled = false;
        answer.enabled = false;
    }

    public async UniTask Q_Displaycontrol()
    {
        //UniTaskの関数をキャンセル可能にするために、トークンを作る
        cancelToken = new CancellationTokenSource();  
        CancellationToken token = cancelToken.Token;
        audioSource.PlayOneShot(audioSource.clip);
        await Show(qNumText, "問題." + (NowQuestionIndex + 1), token);
        await UniTask.Delay(700);
        sel_1_box.text = MessageGeter.question[NowQuestionIndex].sel_1;
        sel_2_box.text = MessageGeter.question[NowQuestionIndex].sel_2;
        sel_3_box.text = MessageGeter.question[NowQuestionIndex].sel_3;
        sel_4_box.text = MessageGeter.question[NowQuestionIndex].sel_4;
        setButton.timer.GoTimer();
        setButton.InitButton();

        //Show()の実行状態を、UniTask型変数に格納
        task = Show(sentence_box, MessageGeter.question[NowQuestionIndex].sentence, token);   
    }

    public async UniTask NextQuestion(){
        setButton.ButtonNotAct();
        
        if(StoreButtonData.data[NowQuestionIndex].q_correct == true){
            marubatupanel.SetActive(true);
            maru.enabled = true;
            await UniTask.Delay(700);
            maru.enabled = false;
            marubatupanel.SetActive(false);
            correct += 1;
            correctAnsNum.text = "正答数：" + correct + "/" + MessageGeter.question.Length;
            sentence_box.maxVisibleCharacters = MessageGeter.question[NowQuestionIndex].sentence.Length;
            await UniTask.Delay(700);
        }else{
            marubatupanel.SetActive(true);
            batu.enabled = true;
            answer.text = "正解:" + MessageGeter.question[NowQuestionIndex].answer_index.ToString();
            answer.enabled = true;
            await UniTask.Delay(700);
            batu.enabled = false;
            answer.enabled = false;
            marubatupanel.SetActive(false);
            sentence_box.maxVisibleCharacters = MessageGeter.question[NowQuestionIndex].sentence.Length;
            await UniTask.Delay(700);
        }

        NowQuestionIndex++;
        if (NowQuestionIndex+1 > MessageGeter.question.Length)
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
                await UniTask.Delay(700*2);
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
/*        qNumText.text = null;
        sentence_box.text = null;
        sel_1_box.text = null;
        sel_2_box.text = null;
        sel_3_box.text = null;
        sel_4_box.text = null;*/
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
