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


    public async UniTask Q_Displaycontrol()
    {
        cancelToken = new CancellationTokenSource();  
        CancellationToken token = cancelToken.Token;

        await Show(qNumText, "問題." + (NowQuestionIndex + 1), token);
        await UniTask.Delay(700);
        sel_1_box.text = MessageGeter.question[NowQuestionIndex].sel_1;
        sel_2_box.text = MessageGeter.question[NowQuestionIndex].sel_2;
        sel_3_box.text = MessageGeter.question[NowQuestionIndex].sel_3;
        sel_4_box.text = MessageGeter.question[NowQuestionIndex].sel_4;
        setButton.timer.GoTimer();
        setButton.InitButton();
        task = Show(sentence_box, MessageGeter.question[NowQuestionIndex].sentence, token);   
    }

    public async UniTask NextQuestion(){
        setButton.ButtonNotAct();
        
        if(StoreButtonData.data[NowQuestionIndex].q_correct == true){
            correct += 1;
            correctAnsNum.text = "正答数：" + correct + "/" + MessageGeter.question.Length;
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
            if (token.IsCancellationRequested)
            {
                _box.maxVisibleCharacters = 0;
                return;
            }  
        }
        _box.maxVisibleCharacters = _text.Length;
    }
    private async UniTask Show(TextMeshProUGUI _box, string _text)
    {
        for (int i=0; i<_text.Length; i++)
        {
            _box.maxVisibleCharacters = i;
            _box.text = _text;
            await UniTask.Delay(DELAYSHOWMS);
        }
        _box.maxVisibleCharacters = _text.Length;
    }

    // 文章が入るボックスを綺麗にする関数
    public void ClearQuizSet()
    {
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
